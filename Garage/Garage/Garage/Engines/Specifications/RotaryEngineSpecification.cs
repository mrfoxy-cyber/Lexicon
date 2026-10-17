namespace Garage.Engines.Specifications;

public class RotaryEngineSpecification : InternalCombustionEngineSpecification
{
    public int HorsePower { get; init; }
    public int DisplacementCc { get; init; }
    public int RotorCount { get; init; }
}
