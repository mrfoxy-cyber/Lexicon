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
    Employee? GetUserByPersonalNumber(string personalNumber);
    IReadOnlyList<Employee> GetUsersByName(string name);
}

public sealed class EmployeeRegistry : IEmployeeRegistry
{
    private readonly EncryptedAppDataStore _store;
    private readonly string _storagePassword;
    private readonly AppData _data;

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

    public IReadOnlyList<Employee> GetAllUsers() => _data.Employees;

    public Employee? GetUserByPersonalNumber(string personalNumber) =>
        _data.Employees.FirstOrDefault(employee =>
            employee.PersonalNumber.Equals(
                personalNumber,
                StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<Employee> GetUsersByName(string name) =>
        _data.Employees
            .Where(employee => employee.Name.Contains(
                name,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
}
