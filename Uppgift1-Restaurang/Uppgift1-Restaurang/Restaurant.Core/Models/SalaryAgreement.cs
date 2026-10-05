namespace Restaurant.Core.Models;

public sealed class SalaryAgreement
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public SalaryPeriod Period { get; set; }
    public DateOnly EffectiveFrom { get; set; }

    // Exclusive end date: an agreement ending on June 1 is valid through May 31.
    public DateOnly? EffectiveTo { get; set; }
}

public enum SalaryPeriod
{
    Hourly,
    Monthly
}
