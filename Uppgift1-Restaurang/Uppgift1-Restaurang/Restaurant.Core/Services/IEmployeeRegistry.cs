namespace Restaurant.Core.Services;

using Restaurant.Core.Models;
using Restaurant.Core.Storage;

public interface IEmployeeRegistry
{
    Task<Guid> RegisterUserAsync(
        string name,
        decimal initialSalary,
        string personalNumber,
        string countryCode,
        DateOnly initialContractStartDate);

    Task<Guid> AddContractAsync(
        Guid employeeId,
        string role,
        decimal initialSalary,
        SalaryPeriod salaryPeriod,
        DateOnly startDate);

    Task<Guid> UpdateSalaryAsync(
        Guid contractId,
        decimal newAmount,
        DateOnly effectiveFrom);

    IReadOnlyList<Employee> GetAllUsers();
    IReadOnlyList<Employee> GetUsersByPersonalNumber(string personalNumber);
    IReadOnlyList<Employee> GetUsersByName(string name);
}

public sealed class DuplicateEmployeeException : InvalidOperationException
{
    public DuplicateEmployeeException(string personalNumber, string countryCode)
        : base(
            $"An employee with personal number '{personalNumber}' " +
            $"and country code '{countryCode}' is already registered.")
    {
    }
}

public sealed class EmployeeNotFoundException : InvalidOperationException
{
    public EmployeeNotFoundException(Guid employeeId)
        : base($"No employee with id '{employeeId}' was found.")
    {
    }
}

public sealed class ContractNotFoundException : InvalidOperationException
{
    public ContractNotFoundException(Guid contractId)
        : base($"No employment contract with id '{contractId}' was found.")
    {
    }
}

public sealed class EmployeeRegistry : IEmployeeRegistry
{
    private readonly EncryptedAppDataStore _store;
    private readonly string _storagePassword;
    private readonly AppData _data;
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    private EmployeeRegistry(
        EncryptedAppDataStore store,
        string storagePassword,
        AppData data)
    {
        _store = store;
        _storagePassword = storagePassword;
        _data = data;
    }

    public static async Task<EmployeeRegistry> CreateAsync(
        string filePath,
        string storagePassword)
    {
        var store = new EncryptedAppDataStore(filePath);
        AppData data = await store.LoadAsync(storagePassword);

        return new EmployeeRegistry(store, storagePassword, data);
    }

    public async Task<Guid> RegisterUserAsync(
        string name,
        decimal initialSalary,
        string personalNumber,
        string countryCode,
        DateOnly initialContractStartDate)
    {
        name = NormalizeRequired(name, nameof(name));
        personalNumber = NormalizeRequired(personalNumber, nameof(personalNumber));
        countryCode = NormalizeCountryCode(countryCode);
        ValidateSalary(initialSalary, nameof(initialSalary));

        await _writeLock.WaitAsync();

        try
        {
            if (FindEmployee(personalNumber, countryCode) is not null)
                throw new DuplicateEmployeeException(personalNumber, countryCode);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = name,
                PersonalNumber = personalNumber,
                Country = countryCode,
                Contracts =
                [
                    CreateContract(
                        "Employee",
                        initialSalary,
                        SalaryPeriod.Monthly,
                        initialContractStartDate)
                ]
            };

            _data.Employees.Add(employee);

            try
            {
                await SaveAsync();
            }
            catch
            {
                _data.Employees.Remove(employee);
                throw;
            }

            return employee.Id;
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public async Task<Guid> AddContractAsync(
        Guid employeeId,
        string role,
        decimal initialSalary,
        SalaryPeriod salaryPeriod,
        DateOnly startDate)
    {
        role = NormalizeRequired(role, nameof(role));
        ValidateSalary(initialSalary, nameof(initialSalary));

        await _writeLock.WaitAsync();

        try
        {
            Employee employee = _data.Employees.FirstOrDefault(
                employee => employee.Id == employeeId) ??
                throw new EmployeeNotFoundException(employeeId);
            EmploymentContract contract = CreateContract(
                role,
                initialSalary,
                salaryPeriod,
                startDate);

            employee.Contracts.Add(contract);

            try
            {
                await SaveAsync();
            }
            catch
            {
                employee.Contracts.Remove(contract);
                throw;
            }

            return contract.Id;
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public async Task<Guid> UpdateSalaryAsync(
        Guid contractId,
        decimal newAmount,
        DateOnly effectiveFrom)
    {
        ValidateSalary(newAmount, nameof(newAmount));

        await _writeLock.WaitAsync();

        try
        {
            EmploymentContract contract = _data.Employees
                .SelectMany(employee => employee.Contracts)
                .FirstOrDefault(contract => contract.Id == contractId) ??
                throw new ContractNotFoundException(contractId);

            SalaryAgreement currentAgreement = contract.SalaryAgreements
                .OrderByDescending(agreement => agreement.EffectiveFrom)
                .First();

            if (effectiveFrom < currentAgreement.EffectiveFrom)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(effectiveFrom),
                    effectiveFrom,
                    "A new salary cannot start before the latest salary agreement.");
            }

            if (effectiveFrom == currentAgreement.EffectiveFrom)
            {
                decimal oldAmount = currentAgreement.Amount;
                currentAgreement.Amount = newAmount;

                try
                {
                    await SaveAsync();
                }
                catch
                {
                    currentAgreement.Amount = oldAmount;
                    throw;
                }

                return currentAgreement.Id;
            }

            DateOnly? oldEndDate = currentAgreement.EffectiveTo;
            currentAgreement.EffectiveTo = effectiveFrom;

            var newAgreement = new SalaryAgreement
            {
                Id = Guid.NewGuid(),
                Amount = newAmount,
                Period = currentAgreement.Period,
                EffectiveFrom = effectiveFrom
            };
            contract.SalaryAgreements.Add(newAgreement);

            try
            {
                await SaveAsync();
            }
            catch
            {
                contract.SalaryAgreements.Remove(newAgreement);
                currentAgreement.EffectiveTo = oldEndDate;
                throw;
            }

            return newAgreement.Id;
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public IReadOnlyList<Employee> GetAllUsers() => _data.Employees.ToList();

    public IReadOnlyList<Employee> GetUsersByPersonalNumber(
        string personalNumber)
    {
        personalNumber = personalNumber.Trim();

        return _data.Employees
            .Where(employee => employee.PersonalNumber.Equals(
                personalNumber,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public IReadOnlyList<Employee> GetUsersByName(string name)
    {
        name = name.Trim();

        return _data.Employees
            .Where(employee => employee.Name.Contains(
                name,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private Employee? FindEmployee(string personalNumber, string countryCode) =>
        _data.Employees.FirstOrDefault(employee =>
            employee.PersonalNumber.Equals(
                personalNumber,
                StringComparison.OrdinalIgnoreCase) &&
            employee.Country.Equals(
                countryCode,
                StringComparison.OrdinalIgnoreCase));

    private static EmploymentContract CreateContract(
        string role,
        decimal initialSalary,
        SalaryPeriod salaryPeriod,
        DateOnly startDate) =>
        new()
        {
            Id = Guid.NewGuid(),
            Role = role,
            StartDate = startDate,
            SalaryAgreements =
            [
                new SalaryAgreement
                {
                    Id = Guid.NewGuid(),
                    Amount = initialSalary,
                    Period = salaryPeriod,
                    EffectiveFrom = startDate
                }
            ]
        };

    private static string NormalizeRequired(string value, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
        return value.Trim();
    }

    private static string NormalizeCountryCode(string countryCode) =>
        NormalizeRequired(countryCode, nameof(countryCode)).ToUpperInvariant();

    private static void ValidateSalary(decimal salary, string parameterName)
    {
        if (salary < 0)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                salary,
                "Salary cannot be negative.");
        }
    }

    private Task SaveAsync() => _store.SaveAsync(_data, _storagePassword);
}
