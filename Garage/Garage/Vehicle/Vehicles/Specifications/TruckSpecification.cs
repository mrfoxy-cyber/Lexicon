namespace Garage.Vehicles.Specifications;

public class TruckSpecification : VehicleSpecification
{
    public required int NumberOfWheels { get; init; }
    public required int NumberOfAxles { get; init; }
    public required double MaximumLoadKg { get; init; }
}
