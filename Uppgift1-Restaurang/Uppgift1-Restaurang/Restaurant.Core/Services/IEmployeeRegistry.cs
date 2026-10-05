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

    Task<Guid> AddSalaryAgreementAsync(
        Guid contractId,
        DateOnly startDate,
        DateOnly? endDate,
        decimal amount,
        SalaryPeriod salaryPeriod);

    Task UpdateContractStateAsync(Guid contractId, ContractState state);

    Task EndContractAsync(Guid contractId, DateOnly endDate);

    IReadOnlyList<Employee> GetAllUsers();
    Employee GetEmployeeById(Guid employeeId);
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

public sealed class DuplicateEmployeeIdException : InvalidOperationException
{
    public DuplicateEmployeeIdException(Guid employeeId, int matchCount)
        : base(
            $"Data integrity error: {matchCount} employees " +
            $"have the id '{employeeId}'.")
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

public sealed class ContractAlreadyEndedException : InvalidOperationException
{
    public ContractAlreadyEndedException(Guid contractId)
        : base($"Employment contract '{contractId}' has already ended.")
    {
    }
}

public sealed class DuplicateContractIdException : InvalidOperationException
{
    public DuplicateContractIdException(Guid contractId, int matchCount)
        : base(
            $"Data integrity error: {matchCount} employment contracts " +
            $"have the id '{contractId}'.")
    {
    }
}

public sealed class ApplicableSalaryAgreementNotFoundException : InvalidOperationException
{
    public ApplicableSalaryAgreementNotFoundException(
        Guid contractId,
        DateOnly date)
        : base(
            $"Contract '{contractId}' has no salary agreement applicable " +
            $"on {date:yyyy-MM-dd}.")
    {
    }
}

public sealed class MultipleApplicableSalaryAgreementsException : InvalidOperationException
{
    public MultipleApplicableSalaryAgreementsException(
        Guid contractId,
        DateOnly date,
        int matchCount)
        : base(
            $"Data integrity error: contract '{contractId}' has {matchCount} " +
            $"salary agreements applicable on {date:yyyy-MM-dd}.")
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

    public async Task<Guid> AddSalaryAgreementAsync(
        Guid contractId,
        DateOnly startDate,
        DateOnly? endDate,
        decimal amount,
        SalaryPeriod salaryPeriod)
    {
        ValidateSalary(amount, nameof(amount));

        if (!Enum.IsDefined(salaryPeriod))
        {
            throw new ArgumentOutOfRangeException(
                nameof(salaryPeriod),
                salaryPeriod,
                null);
        }

        if (endDate is not null && endDate <= startDate)
        {
            throw new ArgumentOutOfRangeException(
                nameof(endDate),
                endDate,
                "The salary agreement end date must be after its start date.");
        }

        await _writeLock.WaitAsync();

        try
        {
            EmploymentContract contract = GetUniqueContract(contractId);

            if (contract.State != ContractState.Valid)
            {
                throw new InvalidOperationException(
                    $"Salary agreements cannot be added to invalid contract '{contractId}'.");
            }

            if (startDate < contract.StartDate ||
                contract.EndDate is not null && startDate >= contract.EndDate)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startDate),
                    startDate,
                    "The salary agreement must start within the contract period.");
            }

            if (contract.EndDate is not null &&
                (endDate is null || endDate > contract.EndDate))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(endDate),
                    endDate,
                    "The salary agreement cannot continue beyond the contract end date.");
            }

            List<SalaryAgreement> applicableAgreements = contract.SalaryAgreements
                .Where(agreement =>
                    agreement.EffectiveFrom <= startDate &&
                    (agreement.EffectiveTo is null ||
                     startDate < agreement.EffectiveTo))
                .ToList();

            SalaryAgreement currentAgreement = applicableAgreements.Count switch
            {
                0 => throw new ApplicableSalaryAgreementNotFoundException(
                    contractId,
                    startDate),
                1 => applicableAgreements[0],
                _ => throw new MultipleApplicableSalaryAgreementsException(
                    contractId,
                    startDate,
                    applicableAgreements.Count)
            };

            if (startDate <= currentAgreement.EffectiveFrom)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startDate),
                    startDate,
                    "A replacement salary agreement must start after the current one.");
            }

            bool overlapsAnotherAgreement = contract.SalaryAgreements
                .Where(agreement => agreement != currentAgreement)
                .Any(agreement =>
                    (endDate is null || agreement.EffectiveFrom < endDate) &&
                    (agreement.EffectiveTo is null || startDate < agreement.EffectiveTo));

            if (overlapsAnotherAgreement)
            {
                throw new InvalidOperationException(
                    "The new salary agreement would overlap another agreement.");
            }

            DateOnly? oldEndDate = currentAgreement.EffectiveTo;
            currentAgreement.EffectiveTo = startDate;
            var newAgreement = new SalaryAgreement
            {
                Id = Guid.NewGuid(),
                Amount = amount,
                Period = salaryPeriod,
                EffectiveFrom = startDate,
                EffectiveTo = endDate
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

    public async Task UpdateContractStateAsync(
        Guid contractId,
        ContractState state)
    {
        if (!Enum.IsDefined(state))
            throw new ArgumentOutOfRangeException(nameof(state), state, null);

        await _writeLock.WaitAsync();

        try
        {
            EmploymentContract contract = GetUniqueContract(contractId);
            ContractState oldState = contract.State;
            contract.State = state;

            try
            {
                await SaveAsync();
            }
            catch
            {
                contract.State = oldState;
                throw;
            }
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public async Task EndContractAsync(Guid contractId, DateOnly endDate)
    {
        await _writeLock.WaitAsync();

        try
        {
            EmploymentContract contract = GetUniqueContract(contractId);

            if (contract.EndDate is not null)
                throw new ContractAlreadyEndedException(contractId);

            if (endDate <= contract.StartDate)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(endDate),
                    endDate,
                    "The contract end date must be after its start date.");
            }

            SalaryAgreement activeAgreement = contract.SalaryAgreements
                .Single(agreement => agreement.EffectiveTo is null);

            if (endDate <= activeAgreement.EffectiveFrom)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(endDate),
                    endDate,
                    "The contract cannot end before its active salary agreement starts.");
            }

            contract.EndDate = endDate;
            activeAgreement.EffectiveTo = endDate;

            try
            {
                await SaveAsync();
            }
            catch
            {
                contract.EndDate = null;
                activeAgreement.EffectiveTo = null;
                throw;
            }
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public IReadOnlyList<Employee> GetAllUsers() => _data.Employees.ToList();

    public Employee GetEmployeeById(Guid employeeId)
    {
        List<Employee> matches = _data.Employees
            .Where(employee => employee.Id == employeeId)
            .ToList();

        return matches.Count switch
        {
            0 => throw new EmployeeNotFoundException(employeeId),
            1 => matches[0],
            _ => throw new DuplicateEmployeeIdException(
                employeeId,
                matches.Count)
        };
    }

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

    private EmploymentContract GetUniqueContract(Guid contractId)
    {
        List<EmploymentContract> matches = _data.Employees
            .SelectMany(employee => employee.Contracts)
            .Where(contract => contract.Id == contractId)
            .ToList();

        return matches.Count switch
        {
            0 => throw new ContractNotFoundException(contractId),
            1 => matches[0],
            _ => throw new DuplicateContractIdException(
                contractId,
                matches.Count)
        };
    }

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
