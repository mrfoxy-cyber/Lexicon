using Garage.Vehicles.Types;

namespace Garage.Vehicles.Specifications;

public class MotorcycleSpecification : VehicleSpecification
{
    public MotorcycleType MotorcycleType { get; init; }
    public bool HasSidecar { get; init; }
    public int NumberOfWheels { get; init; }
    public int PassengerCapacity { get; init; }
    public int DriverSeats { get; init; }
}
