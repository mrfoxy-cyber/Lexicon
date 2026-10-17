namespace Garage.Vehicles.Specifications;

public class TruckSpecification : VehicleSpecification
{
    public int NumberOfWheels { get; init; }
    public int NumberOfAxles { get; init; }
    public double MaximumLoadKg { get; init; }
}