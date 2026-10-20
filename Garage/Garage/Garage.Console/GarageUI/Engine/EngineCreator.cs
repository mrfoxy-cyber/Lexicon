using Garage.ConsoleApp.ConsoleFramework;
using Garage.Engines;
using Garage.Vehicles.Specifications;

namespace Garage.ConsoleApp.GarageUI;

public sealed class EngineCreator
{
    private readonly EngineActions engineActions = new();

    public EngineCreatorResult CreateEngines(
        int minimumNumberOfEngines = 0)
    {
        int numberOfEngines = ConsoleInput.ReadInt(
            "Number of engines: ",
            minimumNumberOfEngines,
            VehicleSpecification.MaximumNumberOfEngines);

        Engine[] engines = new Engine[numberOfEngines];

        for (int index = 0; index < engines.Length; index++)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Creating engine {index + 1} of {engines.Length}");

            Engine? engine = ChooseAndCreateEngine();

            if (engine is null)
            {
                return EngineCreatorResult.Cancel();
            }

            engines[index] = engine;
        }

        return EngineCreatorResult.Completed(engines);
    }

    private Engine? ChooseAndCreateEngine()
    {
        EngineKind? selectedEngine =
            ConsoleInput.ReadEnumOrCancel<EngineKind>(
                "Choose engine type:");

        if (selectedEngine is null)
        {
            return null;
        }

        return selectedEngine.Value switch
        {
            EngineKind.Inline =>
                engineActions.CreateInlineEngine(),

            EngineKind.VEngine =>
                engineActions.CreateVEngine(),

            EngineKind.Flat =>
                engineActions.CreateFlatEngine(),

            EngineKind.Rotary =>
                engineActions.CreateRotaryEngine(),

            EngineKind.ElectricMotor =>
                engineActions.CreateElectricMotor(),

            EngineKind.Steam =>
                engineActions.CreateSteamEngine(),

            EngineKind.Turbojet =>
                engineActions.CreateTurbojetEngine(),

            EngineKind.Turbofan =>
                engineActions.CreateTurbofanEngine(),

            EngineKind.Turboprop =>
                engineActions.CreateTurbopropEngine(),

            EngineKind.Turboshaft =>
                engineActions.CreateTurboshaftEngine(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(selectedEngine),
                selectedEngine,
                "Unknown engine type.")
        };
    }
}
