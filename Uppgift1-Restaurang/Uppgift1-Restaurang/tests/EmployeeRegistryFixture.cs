namespace Restaurant.Tests;

public sealed class EmployeeRegistryFixture : IAsyncLifetime
{
    public string TestDirectory { get; } = Path.Combine(
        Path.GetTempPath(),
        $"restaurant-tests-{Guid.NewGuid()}");

    public Task InitializeAsync()
    {
        Directory.CreateDirectory(TestDirectory);
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        if (Directory.Exists(TestDirectory))
            Directory.Delete(TestDirectory, recursive: true);

        return Task.CompletedTask;
    }

    public string CreateStorageFilePath() => Path.Combine(
        TestDirectory,
        $"appdata-{Guid.NewGuid()}.bin");
}
