namespace Garage.Vehicles;

public interface IManufacturer
{
    string Manufacturer { get; }
    string Model { get; }
    int ManufacturingYear { get; }
}
