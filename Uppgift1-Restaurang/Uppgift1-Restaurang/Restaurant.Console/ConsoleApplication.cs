namespace Restaurant.ConsoleApp;

using System.Globalization;
using Restaurant.Core.Models;
using Restaurant.Core.Services;

public class ConsoleApplication
{
    private static readonly string StoragePassword =
        Environment.GetEnvironmentVariable("RESTAURANT_STORAGE_PASSWORD") ??
        "development-password-123";
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

                case "addemployee":
                    await RegisterEmployeeAsync();
                    break;

                case "employees":
                    ListEmployees();
                    break;

                case "employee":
                    PrintEmployee();
                    break;

                case "currentsalaries":
                    PrintEmployeesCurrentSalaries();
                    break;

                case "addcontract":
                    await AddContractAsync();
                    break;

                case "addsalary":
                    await AddSalaryAgreementAsync();
                    break;

                case "contractstate":
                    await UpdateContractStateAsync();
                    break;

                case "endcontract":
                    await EndContractAsync();
                    break;

                case "findname":
                    FetchEmployeeByName();
                    break;

                case "findnumber":
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
        Console.WriteLine("  addemployee - Register a new employee.");
        Console.WriteLine("  addcontract - Add another employment contract.");
        Console.WriteLine("  addsalary - Add a dated salary agreement.");
        Console.WriteLine("  contractstate - Mark a contract valid or invalid.");
        Console.WriteLine("  endcontract - End a contract using its id.");
        Console.WriteLine("  employees - List all employees.");
        Console.WriteLine("  employee - Print one employee's complete record.");
        Console.WriteLine("  currentsalaries - Print current salaries.");
        Console.WriteLine("  findname - Find employees by name.");
        Console.WriteLine("  findnumber - Find employees by personal number.");
        Console.WriteLine("  exit - Exit the application.");
    }

    private async Task RegisterEmployeeAsync()
    {
        string name = ReadRequired("Name: ");
        decimal salary = ReadSalary();
        string personalNumber = ReadRequired("Personal number: ");
        string country = ReadRequired("Country code: ");
        DateOnly contractStartDate = ReadDate(
            "Initial contract start date (yyyy-MM-dd): ");

        try
        {
            Guid id = await Registry.RegisterUserAsync(
                name,
                salary,
                personalNumber,
                country,
                contractStartDate);

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
            PrintEmployeeSummary(employee);
    }

    private void PrintEmployee()
    {
        Guid employeeId = ReadGuid("Employee id: ");

        try
        {
            PrintEmployeeDetails(Registry.GetEmployeeById(employeeId));
        }
        catch (Exception exception) when (
            exception is EmployeeNotFoundException or DuplicateEmployeeIdException)
        {
            Console.WriteLine($"Warning: {exception.Message}");
        }
    }

    private void PrintEmployeesCurrentSalaries()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        bool printedAnySalary = false;
        IReadOnlyList<Employee> employees = Registry.GetAllUsers();
        var employeesMissingCurrentContract = new List<Employee>();

        foreach (Employee employee in employees)
        {
            List<(EmploymentContract Contract, SalaryAgreement Agreement)> salaries =
                [];
            List<EmploymentContract> currentContracts = employee.Contracts
                .Where(contract =>
                    contract.State == ContractState.Valid &&
                    contract.StartDate <= today &&
                    (contract.EndDate is null || today < contract.EndDate))
                .ToList();

            if (currentContracts.Count == 0)
            {
                employeesMissingCurrentContract.Add(employee);
                continue;
            }

            foreach (EmploymentContract contract in currentContracts)
            {
                List<SalaryAgreement> applicableAgreements =
                    contract.SalaryAgreements
                        .Where(agreement =>
                            agreement.EffectiveFrom <= today &&
                            (agreement.EffectiveTo is null ||
                             today < agreement.EffectiveTo))
                        .ToList();

                if (applicableAgreements.Count > 1)
                {
                    Console.WriteLine(
                        $"Data error: contract '{contract.Id}' has " +
                        $"{applicableAgreements.Count} current salary agreements.");
                    continue;
                }

                if (applicableAgreements.Count == 1)
                    salaries.Add((contract, applicableAgreements[0]));
            }

            if (salaries.Count == 0)
                continue;

            PrintEmployeeSummary(employee);

            foreach ((EmploymentContract contract, SalaryAgreement agreement) in salaries)
            {
                Console.WriteLine(
                    $"  Contract: {contract.Id} | Role: {contract.Role} | " +
                    $"Salary: {agreement.Amount:N2} {agreement.Period}");
            }

            printedAnySalary = true;
        }

        if (!printedAnySalary)
            Console.WriteLine("No current salaries were found.");

        if (employeesMissingCurrentContract.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Employees missing a current contract:");

            foreach (Employee employee in employeesMissingCurrentContract)
            {
                PrintEmployeeSummary(employee);
                Console.WriteLine("  contract missing");
            }
        }
    }

    private async Task AddContractAsync()
    {
        Guid employeeId = ReadGuid("Employee id: ");
        string role = ReadRequired("Role: ");
        decimal salary = ReadSalary();
        SalaryPeriod salaryPeriod = ReadSalaryPeriod();
        DateOnly startDate = ReadDate("Contract start date (yyyy-MM-dd): ");

        try
        {
            Guid contractId = await Registry.AddContractAsync(
                employeeId,
                role,
                salary,
                salaryPeriod,
                startDate);

            Console.WriteLine($"Contract saved. Id: {contractId}");
        }
        catch (EmployeeNotFoundException exception)
        {
            Console.WriteLine($"Warning: {exception.Message}");
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Could not save contract: {exception.Message}");
        }
    }

    private async Task AddSalaryAgreementAsync()
    {
        Guid contractId = ReadGuid("Contract id: ");
        decimal amount = ReadSalary();
        SalaryPeriod salaryPeriod = ReadSalaryPeriod();
        DateOnly startDate = ReadDate("Salary start date (yyyy-MM-dd): ");
        DateOnly? endDate = ReadOptionalDate(
            "Salary end date (yyyy-MM-dd, blank for ongoing): ");

        try
        {
            Guid agreementId = await Registry.AddSalaryAgreementAsync(
                contractId,
                startDate,
                endDate,
                amount,
                salaryPeriod);
            Console.WriteLine($"Salary agreement saved. Id: {agreementId}");
        }
        catch (Exception exception) when (
            exception is InvalidOperationException or ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Warning: {exception.Message}");
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Could not save salary agreement: {exception.Message}");
        }
    }

    private async Task UpdateContractStateAsync()
    {
        Guid contractId = ReadGuid("Contract id: ");
        ContractState state = ReadContractState();

        try
        {
            await Registry.UpdateContractStateAsync(contractId, state);
            Console.WriteLine($"Contract marked as {state}.");
        }
        catch (ContractNotFoundException exception)
        {
            Console.WriteLine($"Warning: {exception.Message}");
        }
        catch (DuplicateContractIdException exception)
        {
            Console.WriteLine($"Data error: {exception.Message}");
        }
        catch (ArgumentOutOfRangeException exception)
        {
            Console.WriteLine($"Warning: {exception.Message}");
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Could not update contract state: {exception.Message}");
        }
    }

    private async Task EndContractAsync()
    {
        Guid contractId = ReadGuid("Contract id: ");
        DateOnly endDate = ReadDate("Contract end date (yyyy-MM-dd): ");

        try
        {
            await Registry.EndContractAsync(contractId, endDate);
            Console.WriteLine("Contract ended.");
        }
        catch (Exception exception) when (
            exception is ContractNotFoundException or
            ContractAlreadyEndedException or
            DuplicateContractIdException or
            ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Warning: {exception.Message}");
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Could not end contract: {exception.Message}");
        }
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
            PrintEmployeeSummary(employee);
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
            PrintEmployeeSummary(employee);
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

    private static SalaryPeriod ReadSalaryPeriod()
    {
        while (true)
        {
            Console.Write("Salary period (hourly/monthly): ");
            string value = Console.ReadLine()?.Trim() ?? "";

            if (value.Equals("hourly", StringComparison.OrdinalIgnoreCase))
                return SalaryPeriod.Hourly;

            if (value.Equals("monthly", StringComparison.OrdinalIgnoreCase))
                return SalaryPeriod.Monthly;

            Console.WriteLine("Enter 'hourly' or 'monthly'.");
        }
    }

    private static ContractState ReadContractState()
    {
        while (true)
        {
            Console.Write("Contract state (valid/invalid): ");
            string value = Console.ReadLine()?.Trim() ?? "";

            if (Enum.TryParse(value, ignoreCase: true, out ContractState state) &&
                Enum.IsDefined(state))
            {
                return state;
            }

            Console.WriteLine("Enter 'valid' or 'invalid'.");
        }
    }

    private static DateOnly ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (DateOnly.TryParseExact(
                Console.ReadLine(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly date))
            {
                return date;
            }

            Console.WriteLine("Enter a date using yyyy-MM-dd.");
        }
    }

    private static DateOnly? ReadOptionalDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine()?.Trim() ?? "";

            if (value.Length == 0)
                return null;

            if (DateOnly.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly date))
            {
                return date;
            }

            Console.WriteLine("Enter a date using yyyy-MM-dd or leave it blank.");
        }
    }

    private static Guid ReadGuid(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (Guid.TryParse(Console.ReadLine(), out Guid id))
                return id;

            Console.WriteLine("Enter a valid id.");
        }
    }

    private static void PrintEmployeeSummary(Employee employee)
    {
        Console.WriteLine(
            $"Employee id: {employee.Id} | Name: {employee.Name} | " +
            $"Personal number: {employee.PersonalNumber}");
    }

    private static void PrintEmployeeDetails(Employee employee)
    {
        Console.WriteLine($"Employee id: {employee.Id}");
        Console.WriteLine($"Name: {employee.Name}");
        Console.WriteLine($"Personal number: {employee.PersonalNumber}");
        Console.WriteLine($"Country code: {employee.Country}");

        if (employee.Contracts.Count == 0)
        {
            Console.WriteLine("Contracts: none");
            return;
        }

        Console.WriteLine("Contracts:");

        foreach (EmploymentContract contract in employee.Contracts
            .OrderBy(contract => contract.StartDate))
        {
            string contractEnd = contract.EndDate?.ToString("yyyy-MM-dd") ??
                "ongoing";
            Console.WriteLine($"  Contract id: {contract.Id}");
            Console.WriteLine($"  Role: {contract.Role}");
            Console.WriteLine($"  State: {contract.State}");
            Console.WriteLine(
                $"  Period: {contract.StartDate:yyyy-MM-dd} - {contractEnd}");

            if (contract.SalaryAgreements.Count == 0)
            {
                Console.WriteLine("    Salary agreements: none");
                continue;
            }

            Console.WriteLine("    Salary agreements:");

            foreach (SalaryAgreement agreement in contract.SalaryAgreements
                .OrderBy(agreement => agreement.EffectiveFrom))
            {
                string salaryEnd = agreement.EffectiveTo?.ToString("yyyy-MM-dd") ??
                    "ongoing";
                Console.WriteLine(
                    $"      Id: {agreement.Id} | Amount: {agreement.Amount:N2} | " +
                    $"Period: {agreement.Period} | " +
                    $"{agreement.EffectiveFrom:yyyy-MM-dd} - {salaryEnd}");
            }
        }
    }

    private IEmployeeRegistry Registry =>
        _employeeRegistry ?? throw new InvalidOperationException(
            "The employee registry has not been initialized.");
}
