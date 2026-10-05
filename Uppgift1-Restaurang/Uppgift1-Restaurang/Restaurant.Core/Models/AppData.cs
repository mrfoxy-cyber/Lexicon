namespace Restaurant.Core.Models;

public sealed class AppData
{
    public List<Employee> Employees { get; set; } = [];

    // You can add more application data later:
    // public List<Order> Orders { get; set; } = [];
    // public List<MenuItem> MenuItems { get; set; } = [];
}
