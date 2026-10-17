using Garage.Vehicles.Components;

namespace Garage.Vehicles.Specifications;

public class AirplaneSpecification : VehicleSpecification
{
    public double WingspanMeters { get; init; }
    public double MaximumAltitudeMeters { get; init; }
    public int NumberOfWheels { get; init; }
    public required Cabin Cabin { get; init; }
    public Cockpit? Cockpit { get; init; }
}
