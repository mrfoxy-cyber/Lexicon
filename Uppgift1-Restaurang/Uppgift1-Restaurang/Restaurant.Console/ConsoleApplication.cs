namespace Restaurant.ConsoleApp;

using System.Globalization;
using Restaurant.Core.Models;
using Restaurant.Core.Services;

public class ConsoleApplication
{
    private const string StoragePassword = "development-password-123";
    private const string StorageFile = "appdata.bin";

    private bool _isRunning = true;
    private IEmployeeRegistry? _employeeRegistry;

    public async Task RunAsync()
    {
        try
        {
            _employeeRegistry = await EmployeeRegistry.CreateAsync(
                StorageFile,
                StoragePassword);
        }
        catch (Exception exception) when (
            exception is InvalidOperationException or InvalidDataException or IOException)
        {
            Console.WriteLine($"Could not load employee data: {exception.Message}");
            return;
        }

        Console.WriteLine("Restaurant application");
        Console.WriteLine("Type 'help' to see available commands.");

        while (_isRunning)
        {
            Console.WriteLine();
            Console.Write("> ");

            string command = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "";

            switch (command)
            {
                case "help":
                    Help();
                    break;

                case "registeremployee":
                    await RegisterEmployeeAsync();
                    break;

                case "listemployees":
                    ListEmployees();
                    break;

                case "fetchemployeebyname":
                    FetchEmployeeByName();
                    break;

                case "fetchemployeebypersonalnumber":
                    FetchEmployeeByPersonalNumber();
                    break;

                case "exit":
                    _isRunning = false;
                    break;

                case "":
                    break;

                default:
                    Console.WriteLine("Unknown command. Type 'help'.");
                    break;
            }
        }
    }

    private static void Help()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("  help - Show this help message.");
        Console.WriteLine("  registeremployee - Register a new employee.");
        Console.WriteLine("  listemployees - List all employees.");
        Console.WriteLine("  fetchemployeebyname - Fetch employee by name.");
        Console.WriteLine("  fetchemployeebypersonalnumber - Fetch employee by personal number.");
        Console.WriteLine("  exit - Exit the application.");
    }

    private async Task RegisterEmployeeAsync()
    {
        string name = ReadRequired("Name: ");
        decimal salary = ReadSalary();
        string personalNumber = ReadRequired("Personal number: ");
        string country = ReadRequired("Country code: ");

        try
        {
            Guid id = await Registry.RegisterUserAsync(
                name,
                salary,
                personalNumber,
                country);

            Console.WriteLine($"Employee saved. Id: {id}");
        }
        catch (DuplicateEmployeeException exception)
        {
            Console.WriteLine($"Warning: {exception.Message}");
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Could not save employee: {exception.Message}");
        }
    }

    private void ListEmployees()
    {
        IReadOnlyList<Employee> employees = Registry.GetAllUsers();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees have been registered.");
            return;
        }

        foreach (Employee employee in employees)
            PrintEmployee(employee);
    }

    private void FetchEmployeeByName()
    {
        string name = ReadRequired("Name: ");
        IReadOnlyList<Employee> matches = Registry.GetUsersByName(name);

        if (matches.Count == 0)
        {
            Console.WriteLine("No matching employee was found.");
            return;
        }

        foreach (Employee employee in matches)
            PrintEmployee(employee);
    }

    private void FetchEmployeeByPersonalNumber()
    {
        string personalNumber = ReadRequired("Personal number: ");
        IReadOnlyList<Employee> matches =
            Registry.GetUsersByPersonalNumber(personalNumber);

        if (matches.Count == 0)
        {
            Console.WriteLine("No matching employee was found.");
            return;
        }

        foreach (Employee employee in matches)
            PrintEmployee(employee);
    }

    private static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine()?.Trim() ?? "";

            if (value.Length > 0)
                return value;

            Console.WriteLine("A value is required.");
        }
    }

    private static decimal ReadSalary()
    {
        while (true)
        {
            Console.Write("Salary: ");

            if (decimal.TryParse(
                Console.ReadLine(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal salary) && salary >= 0)
            {
                return salary;
            }

            Console.WriteLine("Enter a valid salary (zero or greater).");
        }
    }

    private static void PrintEmployee(Employee employee) =>
        Console.WriteLine(
            $"{employee.Name} | Salary: {employee.Salary:N2} | " +
            $"Personal number: {employee.PersonalNumber} | Country code: {employee.Country}");

    private IEmployeeRegistry Registry =>
        _employeeRegistry ?? throw new InvalidOperationException(
            "The employee registry has not been initialized.");
}
