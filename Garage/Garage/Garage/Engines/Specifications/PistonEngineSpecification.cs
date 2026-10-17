namespace Garage.Engines.Specifications;

public class PistonEngineSpecification : InternalCombustionEngineSpecification
{
    public int HorsePower { get; init; }
    public int DisplacementCc { get; init; }
    public int CylinderCount { get; init; }

    public double? CylinderVolumeCc =>
        CylinderCount > 0
            ? (double)DisplacementCc / CylinderCount
            : null;
}
