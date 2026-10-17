using Garage.Engines;
using Garage.Vehicles.Components;

namespace Garage.Vehicles.Specifications;

public abstract class VehicleSpecification
{
    public double WeightKg { get; init; }
    public required Dimensions Dimensions { get; init; }
    public List<Engine> Engines { get; init; } = [];
}
