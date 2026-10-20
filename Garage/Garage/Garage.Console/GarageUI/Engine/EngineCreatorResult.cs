using Garage.Engines;

namespace Garage.ConsoleApp.GarageUI;

public sealed record EngineCreatorResult(
    bool Success,
    bool Cancelled,
    Engine[] Engines)
{
    public string Message
    {
        get
        {
            if (Cancelled)
            {
                return "Engine creation was cancelled.";
            }

            if (!Success)
            {
                return "The engines could not be created.";
            }

            string engineNames = string.Join(
                ", ",
                Engines.Select(engine =>
                    $"{engine.GetType().Name} " +
                    $"{engine.Model} " +
                    $"({engine.SerialNumber})"));

            return $"Created {Engines.Length} engine(s): {engineNames}";
        }
    }

    public static EngineCreatorResult Completed(Engine[] engines)
    {
        return new EngineCreatorResult(
            Success: true,
            Cancelled: false,
            Engines: engines);
    }

    public static EngineCreatorResult Cancel()
    {
        return new EngineCreatorResult(
            Success: false,
            Cancelled: true,
            Engines: []);
    }
}