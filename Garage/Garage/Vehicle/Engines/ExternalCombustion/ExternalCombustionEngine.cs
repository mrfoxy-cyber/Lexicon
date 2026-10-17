using Garage.Engines;
using Garage.Engines.Specifications;

namespace Garage.Engines.ExternalCombustion;

public abstract class ExternalCombustionEngine<TSpecification> : Engine<TSpecification>
    where TSpecification : ExternalCombustionEngineSpecification
{
}
