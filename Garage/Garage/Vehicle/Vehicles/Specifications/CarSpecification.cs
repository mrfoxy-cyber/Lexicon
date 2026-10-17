using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Vehicles.Specifications;

public class CarSpecification : VehicleSpecification
{
    public BodyType BodyType { get; init; }
    public int NumberOfDoors { get; init; }
    public int NumberOfWheels { get; init; }
    public required Cabin Cabin { get; init; }
}
