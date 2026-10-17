using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Vehicles.Specifications;

public class BoatSpecification : VehicleSpecification
{
    public required BoatType BoatType { get; init; }
    public required double DraftMeters { get; init; }
    public required int PassengerCapacity { get; init; }
    public required int DriverSeats { get; init; }
    public Cabin? Cabin { get; init; }
}
