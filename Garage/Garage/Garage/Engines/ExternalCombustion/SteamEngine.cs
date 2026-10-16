namespace Garage.Engines;

public class SteamEngine : ExternalCombustionEngine
{
    public int HorsePower { get; init; }
    public double BoilerPressureBar { get; init; }
}
