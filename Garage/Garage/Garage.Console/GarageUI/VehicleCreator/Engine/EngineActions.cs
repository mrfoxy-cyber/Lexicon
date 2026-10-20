using Garage.Engines;
using Garage.Engines.Electric;
using Garage.Engines.ExternalCombustion;
using Garage.Engines.InternalCombustion;
using Garage.Engines.InternalCombustion.Piston;
using Garage.Engines.InternalCombustion.Turbine;
using Garage.Factories;

namespace Garage.ConsoleApp.GarageUI;

public sealed class EngineActions
{
    public EngineCreatorResult CreateEngine(EngineDraft draft)
    {
        string[] missing = draft.GetMissingFields();

        if (missing.Length > 0)
        {
            return EngineCreatorResult.Failed(
                $"Complete these engine fields: " +
                string.Join(", ", missing));
        }

        Engine engine = draft.Kind!.Value switch
        {
            EngineKind.Inline =>
                EngineFactory.CreateInlineEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.HorsePower!.Value,
                    draft.DisplacementCc!.Value,
                    draft.CylinderCount!.Value),

            EngineKind.VEngine =>
                EngineFactory.CreateVEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.HorsePower!.Value,
                    draft.DisplacementCc!.Value,
                    draft.CylinderCount!.Value),

            EngineKind.Flat =>
                EngineFactory.CreateFlatEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.HorsePower!.Value,
                    draft.DisplacementCc!.Value,
                    draft.CylinderCount!.Value),

            EngineKind.Rotary =>
                EngineFactory.CreateRotaryEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.HorsePower!.Value,
                    draft.DisplacementCc!.Value,
                    draft.RotorCount!.Value),

            EngineKind.ElectricMotor =>
                EngineFactory.CreateElectricMotor(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.PowerKw!.Value,
                    draft.Voltage!.Value),

            EngineKind.Steam =>
                EngineFactory.CreateSteamEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.HorsePower!.Value,
                    draft.BoilerPressureBar!.Value),

            EngineKind.Turbojet =>
                EngineFactory.CreateTurbojetEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.ThrustKn!.Value),

            EngineKind.Turbofan =>
                EngineFactory.CreateTurbofanEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.ThrustKn!.Value,
                    draft.BypassRatio!.Value),

            EngineKind.Turboprop =>
                EngineFactory.CreateTurbopropEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.ThrustKn!.Value,
                    draft.ShaftHorsePower!.Value),

            EngineKind.Turboshaft =>
                EngineFactory.CreateTurboshaftEngine(
                    draft.SerialNumber!,
                    draft.Model!,
                    draft.FuelType!.Value,
                    draft.ThrustKn!.Value,
                    draft.ShaftHorsePower!.Value),

            _ => throw new ArgumentOutOfRangeException()
        };

        return EngineCreatorResult.Completed([engine]);
    }

    public static void PrintEngine(Engine engine)
    {
        Console.WriteLine($"Type: {engine.GetType().Name}");
        Console.WriteLine($"Serial number: {engine.SerialNumber}");
        Console.WriteLine($"Model: {engine.Model}");

        if (engine.FuelType is not null)
            Console.WriteLine($"Fuel type: {engine.FuelType}");

        switch (engine)
        {
            case PistonEngine piston:
                Console.WriteLine($"Horsepower: {piston.Specification.HorsePower}");
                Console.WriteLine(
                    $"Total displacement: " +
                    $"{piston.Specification.DisplacementCc} cc");
                Console.WriteLine(
                    $"Cylinder count: " +
                    $"{piston.Specification.CylinderCount}");

                if (piston.Specification.CylinderVolumeCc is double cylinderVolumeCc)
                {
                    Console.WriteLine(
                        $"Volume per cylinder: {cylinderVolumeCc:F2} cc");
                }
                break;

            case RotaryEngine rotary:
                Console.WriteLine($"Horsepower: {rotary.Specification.HorsePower}");
                Console.WriteLine($"Displacement: {rotary.Specification.DisplacementCc} cc");
                Console.WriteLine($"Rotors: {rotary.Specification.RotorCount}");
                break;

            case ElectricMotor electricMotor:
                Console.WriteLine($"Power: {electricMotor.Specification.PowerKw} kW");
                Console.WriteLine($"Voltage: {electricMotor.Specification.Voltage} V");
                break;

            case SteamEngine steam:
                Console.WriteLine($"Horsepower: {steam.Specification.HorsePower}");
                Console.WriteLine($"Boiler pressure: {steam.Specification.BoilerPressureBar} bar");
                break;

            case TurbofanEngine turbofan:
                Console.WriteLine($"Thrust: {turbofan.Specification.ThrustKn} kN");
                Console.WriteLine($"Bypass ratio: {turbofan.Specification.BypassRatio}");
                break;

            case TurbopropEngine turboprop:
                Console.WriteLine($"Thrust: {turboprop.Specification.ThrustKn} kN");
                Console.WriteLine($"Shaft horsepower: {turboprop.Specification.ShaftHorsePower}");
                break;

            case TurboshaftEngine turboshaft:
                Console.WriteLine($"Thrust: {turboshaft.Specification.ThrustKn} kN");
                Console.WriteLine($"Shaft horsepower: {turboshaft.Specification.ShaftHorsePower}");
                break;

            case TurbojetEngine turbojet:
                Console.WriteLine($"Thrust: {turbojet.Specification.ThrustKn} kN");
                break;
        }
    }
}
