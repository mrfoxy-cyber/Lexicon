using Garage.Engines;

namespace Garage.Engines.ExternalCombustion;

public abstract class ExternalCombustionEngine : Engine
{
    public FuelType FuelType { get; init; }
}
