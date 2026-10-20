using Garage.Vehicles;
using Garage.Vehicles.Types;

namespace Garage.Search;

public class VehicleSearchCriteria
{
    public string? RegistrationNumber { get; init; }
    public string? Manufacturer { get; init; }
    public string? Model { get; init; }

    public int? ManufacturingYear { get; init; }
    public VehicleColor? Color { get; init; }

    public Type[] VehicleTypes { get; init; } = [];
    public int? NumberOfWheels { get; init; }
    public int? NumberOfEngines { get; init; }
    public double? CylinderVolumeCc { get; init; }
    public Garage.Engines.FuelType? FuelType { get; init; }
    public int? NumberOfSeats { get; init; }
    public double? LengthMeters { get; init; }
}
