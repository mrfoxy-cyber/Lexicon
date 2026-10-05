namespace Restaurant.Core.Models;

public sealed class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string PersonalNumber { get; set; } = "";
    public string Country { get; set; } = "";
    public List<EmploymentContract> Contracts { get; set; } = [];
}
