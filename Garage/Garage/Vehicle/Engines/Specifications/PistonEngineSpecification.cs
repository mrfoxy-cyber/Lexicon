namespace Garage.Engines.Specifications;

public class PistonEngineSpecification : InternalCombustionEngineSpecification
{
    public required int HorsePower { get; init; }
    public required int DisplacementCc { get; init; }
    public required int CylinderCount { get; init; }

    public double? CylinderVolumeCc =>
        CylinderCount > 0
            ? (double)DisplacementCc / CylinderCount
            : null;
}
