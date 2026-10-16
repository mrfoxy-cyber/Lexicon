using Garage.Engines.InternalCombustion;

namespace Garage.Engines.InternalCombustion.Turbine;

public abstract class TurbineEngine : InternalCombustionEngine
{
    public double ThrustKn { get; init; }
}
