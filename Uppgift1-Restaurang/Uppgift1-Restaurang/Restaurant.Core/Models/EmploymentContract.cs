namespace Restaurant.Core.Models;

public sealed class EmploymentContract
{
    public Guid Id { get; set; }
    public string Role { get; set; } = "";
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<SalaryAgreement> SalaryAgreements { get; set; } = [];
}
