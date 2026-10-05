using Restaurant.Core.Models;
using Restaurant.Core.Services;

namespace Restaurant.Tests;

public sealed class EmployeeRegistryTests : IClassFixture<EmployeeRegistryFixture>
{
    private const string Password = "test-password";
    private static readonly DateOnly ContractStartDate = new(2025, 1, 1);
    private readonly EmployeeRegistryFixture _fixture;

    public EmployeeRegistryTests(EmployeeRegistryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task RegisterUserAsync_SavesEmployee()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);

        Guid id = await registry.RegisterUserAsync(
            "Ada Lovelace",
            45_000m,
            "1815-12-10",
            "GB",
            ContractStartDate);

        var employee = Assert.Single(registry.GetAllUsers());
        Assert.Equal(id, employee.Id);
        Assert.Equal("Ada Lovelace", employee.Name);
        Assert.Equal(45_000m, GetOnlySalary(employee).Amount);
        Assert.Equal("1815-12-10", employee.PersonalNumber);
        Assert.Equal("GB", employee.Country);
        Assert.Equal(ContractStartDate, Assert.Single(employee.Contracts).StartDate);
        Assert.Equal(ContractStartDate, GetOnlySalary(employee).EffectiveFrom);
        Assert.True(File.Exists(filePath));
    }

    [Fact]
    public async Task CreateAsync_LoadsEmployeesSavedByEarlierRegistry()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry firstRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);

        Guid id = await firstRegistry.RegisterUserAsync(
            "Grace Hopper",
            52_000m,
            "1906-12-09",
            "United States",
            ContractStartDate);

        EmployeeRegistry reloadedRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);

        var employee = Assert.Single(reloadedRegistry.GetAllUsers());
        Assert.Equal(id, employee.Id);
        Assert.Equal("Grace Hopper", employee.Name);
    }

    [Fact]
    public async Task Lookups_AreCaseInsensitive()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "Katherine Johnson",
            48_000m,
            "1918-08-26",
            "United States",
            ContractStartDate);

        var byPersonalNumber = registry.GetUsersByPersonalNumber("1918-08-26");
        var byPartialName = registry.GetUsersByName("JOHNSON");

        var employee = Assert.Single(byPersonalNumber);
        Assert.Equal("Katherine Johnson", employee.Name);
        Assert.Single(byPartialName);
        Assert.Equal(employee.Id, byPartialName[0].Id);
    }

    [Fact]
    public async Task CreateAsync_WithWrongPassword_Throws()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        await registry.RegisterUserAsync(
            "Margaret Hamilton",
            50_000m,
            "1936-08-17",
            "United States",
            ContractStartDate);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            EmployeeRegistry.CreateAsync(filePath, "wrong-password"));
    }

    [Fact]
    public async Task RegisterUserAsync_WhenSaveFails_DoesNotKeepEmployeeInMemory()
    {
        string missingDirectory = Path.Combine(
            _fixture.TestDirectory,
            "missing-directory");
        string filePath = Path.Combine(missingDirectory, "appdata.bin");
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);

        await Assert.ThrowsAsync<DirectoryNotFoundException>(() =>
            registry.RegisterUserAsync(
                "Dorothy Vaughan",
                47_000m,
                "1910-09-20",
                "United States",
                ContractStartDate));

        Assert.Empty(registry.GetAllUsers());
    }

    [Fact]
    public async Task RegisterUserAsync_WithZeroSalary_SavesEmployee()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);

        await registry.RegisterUserAsync(
            "New Starter",
            0m,
            "2000-01-01",
            "Sweden",
            ContractStartDate);

        var employee = Assert.Single(registry.GetAllUsers());
        Assert.Equal(0m, GetOnlySalary(employee).Amount);
    }

    [Fact]
    public async Task RegisterUserAsync_WithSalaryBelowZero_Throws()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);

        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            registry.RegisterUserAsync(
                "New Starter",
                -0.01m,
                "2000-01-01",
                "Sweden",
                ContractStartDate));

        Assert.Equal("initialSalary", exception.ParamName);
        Assert.Empty(registry.GetAllUsers());
    }

    [Theory]
    [InlineData(" ", "2000-01-01", "Sweden", "name")]
    [InlineData("New Starter", " ", "Sweden", "personalNumber")]
    [InlineData("New Starter", "2000-01-01", " ", "countryCode")]
    public async Task RegisterUserAsync_WithWhitespaceRequiredField_Throws(
        string name,
        string personalNumber,
        string country,
        string expectedParameter)
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            registry.RegisterUserAsync(
                name,
                1m,
                personalNumber,
                country,
                ContractStartDate));

        Assert.Equal(expectedParameter, exception.ParamName);
        Assert.Empty(registry.GetAllUsers());
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingPersonalNumberAndCountryCode_ThrowsWarningException()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "First Employee",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);

        var exception = await Assert.ThrowsAsync<DuplicateEmployeeException>(() =>
            registry.RegisterUserAsync(
                "Duplicate Employee",
                41_000m,
                "2000-01-01",
                "se",
                ContractStartDate));

        Assert.Contains("2000-01-01", exception.Message);
        Assert.Contains("SE", exception.Message);
        Assert.Single(registry.GetAllUsers());
    }

    [Fact]
    public async Task RegisterUserAsync_WithSamePersonalNumberAndDifferentCountryCode_SavesEmployee()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "Swedish Employee",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);

        await registry.RegisterUserAsync(
            "Norwegian Employee",
            41_000m,
            "2000-01-01",
            "NO",
            ContractStartDate);

        Assert.Equal(2, registry.GetAllUsers().Count);
    }

    [Fact]
    public async Task GetUsersByPersonalNumber_ReturnsAllMatchingCountries()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "Swedish Employee",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        await registry.RegisterUserAsync(
            "Norwegian Employee",
            41_000m,
            "2000-01-01",
            "NO",
            ContractStartDate);
        await registry.RegisterUserAsync(
            "Someone Else",
            42_000m,
            "1999-01-01",
            "SE",
            ContractStartDate);

        IReadOnlyList<Restaurant.Core.Models.Employee> matches =
            registry.GetUsersByPersonalNumber("2000-01-01");

        Assert.Equal(2, matches.Count);
        Assert.Contains(matches, employee => employee.Country == "SE");
        Assert.Contains(matches, employee => employee.Country == "NO");
    }

    [Fact]
    public async Task RegisterUserAsync_ConcurrentDuplicate_StoresOnlyOneEmployee()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        var startSignal = new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        Task<Exception?> firstRegistration = CaptureRegistrationAsync(
            startSignal.Task,
            registry,
            "First Employee",
            40_000m);
        Task<Exception?> secondRegistration = CaptureRegistrationAsync(
            startSignal.Task,
            registry,
            "Second Employee",
            40_000m);

        startSignal.SetResult(true);
        Exception?[] outcomes = await Task.WhenAll(
            firstRegistration,
            secondRegistration);

        Assert.Single(outcomes, outcome => outcome is null);
        Exception duplicateOutcome = Assert.Single(
            outcomes.OfType<DuplicateEmployeeException>());
        Assert.IsType<DuplicateEmployeeException>(duplicateOutcome);
        Assert.Single(registry.GetAllUsers());

        EmployeeRegistry reloadedRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        Assert.Single(reloadedRegistry.GetAllUsers());
    }

    [Fact]
    public async Task RegisterUserAsync_ConcurrentSameEmployeeWithDifferentSalary_FirstStartedWins()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        Task<Exception?> firstRegistration = CaptureRegistrationAsync(
            Task.CompletedTask,
            registry,
            "Same Employee",
            40_000m);
        Task<Exception?> secondRegistration = CaptureRegistrationAsync(
            Task.CompletedTask,
            registry,
            "Same Employee",
            41_000m);

        Exception?[] outcomes = await Task.WhenAll(
            firstRegistration,
            secondRegistration);

        Assert.Null(outcomes[0]);
        Assert.IsType<DuplicateEmployeeException>(outcomes[1]);

        var employee = Assert.Single(registry.GetAllUsers());
        Assert.Equal("Same Employee", employee.Name);
        Assert.Equal(40_000m, GetOnlySalary(employee).Amount);
        Assert.Equal("2000-01-01", employee.PersonalNumber);
        Assert.Equal("SE", employee.Country);

        EmployeeRegistry reloadedRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        Assert.Single(reloadedRegistry.GetAllUsers());
    }

    [Fact]
    public async Task AddContractAsync_AllowsMultipleOngoingContracts()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        Guid employeeId = await registry.RegisterUserAsync(
            "Multiple Roles",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);

        Guid contractId = await registry.AddContractAsync(
            employeeId,
            "Weekend waiter",
            180m,
            SalaryPeriod.Hourly,
            new DateOnly(2026, 1, 1));

        Employee employee = Assert.Single(registry.GetAllUsers());
        Assert.Equal(2, employee.Contracts.Count);
        EmploymentContract addedContract = Assert.Single(
            employee.Contracts,
            contract => contract.Id == contractId);
        Assert.Equal("Weekend waiter", addedContract.Role);
        Assert.Equal(SalaryPeriod.Hourly, Assert.Single(
            addedContract.SalaryAgreements).Period);
    }

    [Fact]
    public async Task AddContractAsync_WithUnknownEmployeeId_Throws()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);

        await Assert.ThrowsAsync<EmployeeNotFoundException>(() =>
            registry.AddContractAsync(
                Guid.NewGuid(),
                "Waiter",
                180m,
                SalaryPeriod.Hourly,
                new DateOnly(2026, 1, 1)));
    }

    [Fact]
    public async Task AddSalaryAgreementAsync_ClosesCurrentAndAddsReplacement()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        await registry.RegisterUserAsync(
            "Salary Change",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        EmploymentContract contract = Assert.Single(
            Assert.Single(registry.GetAllUsers()).Contracts);
        SalaryAgreement originalAgreement = Assert.Single(
            contract.SalaryAgreements);
        DateOnly newStartDate = ContractStartDate.AddYears(1);
        DateOnly newEndDate = newStartDate.AddYears(1);

        Guid agreementId = await registry.AddSalaryAgreementAsync(
            contract.Id,
            newStartDate,
            newEndDate,
            42_000m,
            SalaryPeriod.Monthly);

        Assert.Equal(newStartDate, originalAgreement.EffectiveTo);
        SalaryAgreement newAgreement = Assert.Single(
            contract.SalaryAgreements,
            agreement => agreement.Id == agreementId);
        Assert.Equal(42_000m, newAgreement.Amount);
        Assert.Equal(SalaryPeriod.Monthly, newAgreement.Period);
        Assert.Equal(newStartDate, newAgreement.EffectiveFrom);
        Assert.Equal(newEndDate, newAgreement.EffectiveTo);

        EmployeeRegistry reloadedRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        Assert.Equal(
            2,
            Assert.Single(Assert.Single(
                reloadedRegistry.GetAllUsers()).Contracts).SalaryAgreements.Count);
    }

    [Fact]
    public async Task AddSalaryAgreementAsync_WithMultipleApplicableAgreements_Throws()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "Broken Salary Data",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        EmploymentContract contract = Assert.Single(
            Assert.Single(registry.GetAllUsers()).Contracts);
        contract.SalaryAgreements.Add(new SalaryAgreement
        {
            Id = Guid.NewGuid(),
            Amount = 41_000m,
            Period = SalaryPeriod.Monthly,
            EffectiveFrom = ContractStartDate
        });

        await Assert.ThrowsAsync<MultipleApplicableSalaryAgreementsException>(() =>
            registry.AddSalaryAgreementAsync(
                contract.Id,
                ContractStartDate.AddYears(1),
                null,
                42_000m,
                SalaryPeriod.Monthly));

        Assert.Equal(2, contract.SalaryAgreements.Count);
    }

    [Fact]
    public async Task AddSalaryAgreementAsync_WithNoApplicableAgreement_Throws()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "Salary Gap",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        EmploymentContract contract = Assert.Single(
            Assert.Single(registry.GetAllUsers()).Contracts);
        Assert.Single(contract.SalaryAgreements).EffectiveTo =
            ContractStartDate.AddMonths(1);

        await Assert.ThrowsAsync<ApplicableSalaryAgreementNotFoundException>(() =>
            registry.AddSalaryAgreementAsync(
                contract.Id,
                ContractStartDate.AddYears(1),
                null,
                42_000m,
                SalaryPeriod.Monthly));
    }

    [Fact]
    public async Task UpdateContractStateAsync_MarksContractInvalidAndPersists()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        await registry.RegisterUserAsync(
            "Invalid Contract",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        EmploymentContract contract = Assert.Single(
            Assert.Single(registry.GetAllUsers()).Contracts);

        await registry.UpdateContractStateAsync(
            contract.Id,
            ContractState.Invalid);

        Assert.Equal(ContractState.Invalid, contract.State);
        Assert.Single(contract.SalaryAgreements);

        EmployeeRegistry reloadedRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        EmploymentContract reloadedContract = Assert.Single(
            Assert.Single(reloadedRegistry.GetAllUsers()).Contracts);
        Assert.Equal(ContractState.Invalid, reloadedContract.State);
        Assert.Single(reloadedContract.SalaryAgreements);
    }

    [Fact]
    public async Task UpdateContractStateAsync_WithUnknownContract_Throws()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);

        await Assert.ThrowsAsync<ContractNotFoundException>(() =>
            registry.UpdateContractStateAsync(
                Guid.NewGuid(),
                ContractState.Invalid));
    }

    [Fact]
    public async Task EndContractAsync_ById_ClosesContractAndSalaryAgreement()
    {
        string filePath = _fixture.CreateStorageFilePath();
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        await registry.RegisterUserAsync(
            "Ending Employee",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        EmploymentContract contract = Assert.Single(
            Assert.Single(registry.GetAllUsers()).Contracts);
        DateOnly endDate = ContractStartDate.AddYears(1);

        await registry.EndContractAsync(contract.Id, endDate);

        Assert.Equal(endDate, contract.EndDate);
        Assert.Equal(endDate, Assert.Single(
            contract.SalaryAgreements).EffectiveTo);

        EmployeeRegistry reloadedRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        EmploymentContract reloadedContract = Assert.Single(
            Assert.Single(reloadedRegistry.GetAllUsers()).Contracts);
        Assert.Equal(endDate, reloadedContract.EndDate);
        Assert.Equal(endDate, Assert.Single(
            reloadedContract.SalaryAgreements).EffectiveTo);
    }

    [Fact]
    public async Task EndContractAsync_WithUnknownContract_Throws()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);

        await Assert.ThrowsAsync<ContractNotFoundException>(() =>
            registry.EndContractAsync(
                Guid.NewGuid(),
                ContractStartDate.AddYears(1)));
    }

    [Fact]
    public async Task EndContractAsync_OnStartDate_Throws()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "Ending Employee",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        EmploymentContract contract = Assert.Single(
            Assert.Single(registry.GetAllUsers()).Contracts);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            registry.EndContractAsync(contract.Id, ContractStartDate));

        Assert.Null(contract.EndDate);
        Assert.Null(Assert.Single(contract.SalaryAgreements).EffectiveTo);
    }

    [Fact]
    public async Task UpdateContractStateAsync_WithDuplicateContractId_ThrowsDataIntegrityError()
    {
        EmployeeRegistry registry = await EmployeeRegistry.CreateAsync(
            _fixture.CreateStorageFilePath(),
            Password);
        await registry.RegisterUserAsync(
            "First Employee",
            40_000m,
            "2000-01-01",
            "SE",
            ContractStartDate);
        await registry.RegisterUserAsync(
            "Second Employee",
            41_000m,
            "2001-01-01",
            "SE",
            ContractStartDate);

        IReadOnlyList<Employee> employees = registry.GetAllUsers();
        Guid duplicateId = employees[0].Contracts[0].Id;
        employees[1].Contracts[0].Id = duplicateId;

        DuplicateContractIdException exception =
            await Assert.ThrowsAsync<DuplicateContractIdException>(() =>
                registry.UpdateContractStateAsync(
                    duplicateId,
                    ContractState.Invalid));

        Assert.Contains("2 employment contracts", exception.Message);
    }

    private static async Task<Exception?> CaptureRegistrationAsync(
        Task startSignal,
        EmployeeRegistry registry,
        string name,
        decimal salary)
    {
        await startSignal;

        try
        {
            await registry.RegisterUserAsync(
                name,
                salary,
                "2000-01-01",
                "SE",
                ContractStartDate);
            return null;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    private static SalaryAgreement GetOnlySalary(Employee employee) =>
        Assert.Single(Assert.Single(employee.Contracts).SalaryAgreements);
}
