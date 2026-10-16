namespace Garage.Engines;

public class RotaryEngine : InternalCombustionEngine
{
    public int HorsePower { get; init; }
    public int DisplacementCc { get; init; }
    public int RotorCount { get; init; }
}
