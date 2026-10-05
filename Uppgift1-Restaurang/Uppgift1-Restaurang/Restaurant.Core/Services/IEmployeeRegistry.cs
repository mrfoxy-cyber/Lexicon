namespace Restaurant.Core.Services;

using Restaurant.Core.Models;
using Restaurant.Core.Storage;

public interface IEmployeeRegistry
{
    Task<Guid> RegisterUserAsync(
        string name,
        decimal salary,
        string personalNumber,
        string country);

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

public sealed class EmployeeRegistry : IEmployeeRegistry
{
    private readonly EncryptedAppDataStore _store;
    private readonly string _storagePassword;
    private readonly AppData _data;
    private readonly SemaphoreSlim _registrationLock = new(1, 1);

    private EmployeeRegistry(
        EncryptedAppDataStore store,
        string storagePassword,
        AppData data)
    {
        _store = store;
        _storagePassword = storagePassword;
        _data = data;
    }

    // Constructors cannot be async. An async factory method can load the file
    // before it returns a ready-to-use registry.
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
        decimal salary,
        string personalNumber,
        string country)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(personalNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);

        if (salary < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(salary),
                salary,
                "Salary cannot be negative.");
        }

        await _registrationLock.WaitAsync();

        try
        {
            bool employeeAlreadyExists = _data.Employees.Any(employee =>
                employee.PersonalNumber.Equals(
                    personalNumber,
                    StringComparison.OrdinalIgnoreCase) &&
                employee.Country.Equals(
                    country,
                    StringComparison.OrdinalIgnoreCase));

            if (employeeAlreadyExists)
                throw new DuplicateEmployeeException(personalNumber, country);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = name,
                Salary = salary,
                PersonalNumber = personalNumber,
                Country = country
            };

            _data.Employees.Add(employee);

            try
            {
                await _store.SaveAsync(_data, _storagePassword);
            }
            catch
            {
                // Keep memory and disk consistent if saving fails.
                _data.Employees.Remove(employee);
                throw;
            }

            return employee.Id;
        }
        finally
        {
            _registrationLock.Release();
        }
    }

    public IReadOnlyList<Employee> GetAllUsers() => _data.Employees;

    public IReadOnlyList<Employee> GetUsersByPersonalNumber(
        string personalNumber) =>
        _data.Employees
            .Where(employee => employee.PersonalNumber.Equals(
                personalNumber,
                StringComparison.OrdinalIgnoreCase))
            .ToList();

    public IReadOnlyList<Employee> GetUsersByName(string name) =>
        _data.Employees
            .Where(employee => employee.Name.Contains(
                name,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
}
