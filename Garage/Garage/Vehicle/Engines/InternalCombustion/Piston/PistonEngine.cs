using Garage.Engines.InternalCombustion;
using Garage.Engines.Specifications;

namespace Garage.Engines.InternalCombustion.Piston;

public abstract class PistonEngine : InternalCombustionEngine<PistonEngineSpecification>
{
    public override double? CylinderVolumeCc =>
        Specification.CylinderVolumeCc;
}
