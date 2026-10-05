using Restaurant.Core.Services;

namespace Restaurant.Tests;

public sealed class EmployeeRegistryTests : IClassFixture<EmployeeRegistryFixture>
{
    private const string Password = "test-password";
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
            "United Kingdom");

        var employee = Assert.Single(registry.GetAllUsers());
        Assert.Equal(id, employee.Id);
        Assert.Equal("Ada Lovelace", employee.Name);
        Assert.Equal(45_000m, employee.Salary);
        Assert.Equal("1815-12-10", employee.PersonalNumber);
        Assert.Equal("United Kingdom", employee.Country);
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
            "United States");

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
            "United States");

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
            "United States");

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
                "United States"));

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
            "Sweden");

        var employee = Assert.Single(registry.GetAllUsers());
        Assert.Equal(0m, employee.Salary);
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
                "Sweden"));

        Assert.Equal("salary", exception.ParamName);
        Assert.Empty(registry.GetAllUsers());
    }

    [Theory]
    [InlineData(" ", "2000-01-01", "Sweden", "name")]
    [InlineData("New Starter", " ", "Sweden", "personalNumber")]
    [InlineData("New Starter", "2000-01-01", " ", "country")]
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
                country));

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
            "SE");

        var exception = await Assert.ThrowsAsync<DuplicateEmployeeException>(() =>
            registry.RegisterUserAsync(
                "Duplicate Employee",
                41_000m,
                "2000-01-01",
                "se"));

        Assert.Contains("2000-01-01", exception.Message);
        Assert.Contains("se", exception.Message);
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
            "SE");

        await registry.RegisterUserAsync(
            "Norwegian Employee",
            41_000m,
            "2000-01-01",
            "NO");

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
            "SE");
        await registry.RegisterUserAsync(
            "Norwegian Employee",
            41_000m,
            "2000-01-01",
            "NO");
        await registry.RegisterUserAsync(
            "Someone Else",
            42_000m,
            "1999-01-01",
            "SE");

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
    public async Task RegisterUserAsync_ConcurrentSameEmployeeWithDifferentSalary_OneRegistrationFails()
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
            "Same Employee",
            40_000m);
        Task<Exception?> secondRegistration = CaptureRegistrationAsync(
            startSignal.Task,
            registry,
            "Same Employee",
            41_000m);

        startSignal.SetResult(true);
        Exception?[] outcomes = await Task.WhenAll(
            firstRegistration,
            secondRegistration);

        Assert.Single(outcomes, outcome => outcome is null);
        Exception failedRegistration = Assert.Single(
            outcomes,
            outcome => outcome is not null)!;
        Assert.IsType<DuplicateEmployeeException>(failedRegistration);

        var employee = Assert.Single(registry.GetAllUsers());
        Assert.Equal("Same Employee", employee.Name);
        Assert.Contains(employee.Salary, new[] { 40_000m, 41_000m });
        Assert.Equal("2000-01-01", employee.PersonalNumber);
        Assert.Equal("SE", employee.Country);

        EmployeeRegistry reloadedRegistry = await EmployeeRegistry.CreateAsync(
            filePath,
            Password);
        Assert.Single(reloadedRegistry.GetAllUsers());
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
                "SE");
            return null;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
}
