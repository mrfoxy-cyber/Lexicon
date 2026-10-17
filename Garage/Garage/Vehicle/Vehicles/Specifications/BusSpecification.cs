using Garage.Vehicles.Components;

namespace Garage.Vehicles.Specifications;

public class BusSpecification : VehicleSpecification
{
    public required int NumberOfDoors { get; init; }
    public required int NumberOfWheels { get; init; }
    public required int StandingPassengerCapacity { get; init; }
    public required Cabin Cabin { get; init; }
}
