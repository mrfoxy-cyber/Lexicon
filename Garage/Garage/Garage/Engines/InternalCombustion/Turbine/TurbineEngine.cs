using Garage.Engines.InternalCombustion;
using Garage.Engines.Specifications;

namespace Garage.Engines.InternalCombustion.Turbine;

public abstract class TurbineEngine<TSpecification> : InternalCombustionEngine<TSpecification>
    where TSpecification : TurbineEngineSpecification
{
}
