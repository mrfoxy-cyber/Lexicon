namespace Garage.Engines.Specifications;

public class SteamEngineSpecification : ExternalCombustionEngineSpecification
{
    public int HorsePower { get; init; }
    public double BoilerPressureBar { get; init; }
}
