namespace Garage.Engines;

public abstract class ElectricEngine : Engine
{
    public double PowerKw { get; init; }
    public double Voltage { get; init; }
}
