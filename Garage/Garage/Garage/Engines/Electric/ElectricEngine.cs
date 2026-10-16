using Garage.Engines;

namespace Garage.Engines.Electric;

public abstract class ElectricEngine : Engine
{
    public double PowerKw { get; init; }
    public double Voltage { get; init; }
}
