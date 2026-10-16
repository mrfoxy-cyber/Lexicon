using Garage.Engines.InternalCombustion;

namespace Garage.Engines.InternalCombustion.Piston;

public abstract class PistonEngine : InternalCombustionEngine
{
    public int HorsePower { get; init; }
    public int DisplacementCc { get; init; }
    public int CylinderCount { get; init; }

    public double? CylinderVolumeCc =>
        CylinderCount > 0
            ? (double)DisplacementCc / CylinderCount
            : null;
}
