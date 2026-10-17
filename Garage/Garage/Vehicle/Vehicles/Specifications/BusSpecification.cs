using Garage.Vehicles.Components;

namespace Garage.Vehicles.Specifications;

public class BusSpecification : VehicleSpecification
{
    public int NumberOfDoors { get; init; }
    public int NumberOfWheels { get; init; }
    public int StandingPassengerCapacity { get; init; }
    public required Cabin Cabin { get; init; }
}
