using Garage.Engines;

namespace Garage.Engines.InternalCombustion;

public abstract class InternalCombustionEngine : Engine
{
    public FuelType FuelType { get; init; }
}
