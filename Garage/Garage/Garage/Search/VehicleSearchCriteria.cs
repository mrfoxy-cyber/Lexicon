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

    public Type? VehicleType { get; init; }
    public int? NumberOfWheels { get; init; }
}
