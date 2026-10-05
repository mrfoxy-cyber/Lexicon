namespace Restaurant.Core.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Salary { get; set; }
        public string PersonalNumber { get; set; } = "";
        public string Country { get; set; } = "";
    }
}
