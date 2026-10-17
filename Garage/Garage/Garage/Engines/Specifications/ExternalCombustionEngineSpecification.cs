namespace Garage.Engines.Specifications;

public abstract class ExternalCombustionEngineSpecification : EngineSpecification
{
    public FuelType FuelType { get; init; }
}
