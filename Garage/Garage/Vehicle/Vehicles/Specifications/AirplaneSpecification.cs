using Garage.Vehicles.Components;

namespace Garage.Vehicles.Specifications;

public class AirplaneSpecification : VehicleSpecification
{
    public required double WingspanMeters { get; init; }
    public required double MaximumAltitudeMeters { get; init; }
    public required int NumberOfWheels { get; init; }
    public required Cabin Cabin { get; init; }
    public Cockpit? Cockpit { get; init; }
}
