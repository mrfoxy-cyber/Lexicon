using Garage.Engines;
using Garage.Engines.Specifications;

namespace Garage.Engines.InternalCombustion;

public abstract class InternalCombustionEngine<TSpecification> : Engine<TSpecification>
    where TSpecification : InternalCombustionEngineSpecification
{
    public override FuelType? FuelType => Specification.FuelType;
}
