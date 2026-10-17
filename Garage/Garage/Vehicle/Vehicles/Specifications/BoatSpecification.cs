using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Vehicles.Specifications;

public class BoatSpecification : VehicleSpecification
{
    public BoatType BoatType { get; init; }
    public double DraftMeters { get; init; }
    public int PassengerCapacity { get; init; }
    public int DriverSeats { get; init; }
    public Cabin? Cabin { get; init; }
}
