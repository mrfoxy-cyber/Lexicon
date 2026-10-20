using Garage.ConsoleApp.ConsoleFramework;
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
    public Engine CreateInlineEngine()
    {
        EngineIdentity identity = ReadIdentity();
        PistonInput input = ReadPistonInput();

        return EngineFactory.CreateInlineEngine(
            identity.SerialNumber,
            identity.Model,
            input.FuelType,
            input.HorsePower,
            input.DisplacementCc,
            input.CylinderCount);
    }

    public Engine CreateVEngine()
    {
        EngineIdentity identity = ReadIdentity();
        PistonInput input = ReadPistonInput();

        return EngineFactory.CreateVEngine(
            identity.SerialNumber,
            identity.Model,
            input.FuelType,
            input.HorsePower,
            input.DisplacementCc,
            input.CylinderCount);
    }

    public Engine CreateFlatEngine()
    {
        EngineIdentity identity = ReadIdentity();
        PistonInput input = ReadPistonInput();

        return EngineFactory.CreateFlatEngine(
            identity.SerialNumber,
            identity.Model,
            input.FuelType,
            input.HorsePower,
            input.DisplacementCc,
            input.CylinderCount);
    }

    public Engine CreateRotaryEngine()
    {
        EngineIdentity identity = ReadIdentity();
        FuelType fuelType = ReadFuelType();
        int horsePower = ReadHorsePower();
        int displacementCc = ReadDisplacementCc();
        int rotorCount = ConsoleInput.ReadInt(
            "Number of rotors: ", 1, 10);

        return EngineFactory.CreateRotaryEngine(
            identity.SerialNumber,
            identity.Model,
            fuelType,
            horsePower,
            displacementCc,
            rotorCount);
    }

    public Engine CreateElectricMotor()
    {
        EngineIdentity identity = ReadIdentity();
        double powerKw = ConsoleInput.ReadDouble(
            "Power in kW: ", 0.1, 100_000);
        double voltage = ConsoleInput.ReadDouble(
            "Voltage: ", 1, 100_000);

        return EngineFactory.CreateElectricMotor(
            identity.SerialNumber,
            identity.Model,
            powerKw,
            voltage);
    }

    public Engine CreateSteamEngine()
    {
        EngineIdentity identity = ReadIdentity();
        FuelType fuelType = ReadFuelType();
        int horsePower = ReadHorsePower();
        double boilerPressureBar = ConsoleInput.ReadDouble(
            "Boiler pressure in bar: ", 0.1, 10_000);

        return EngineFactory.CreateSteamEngine(
            identity.SerialNumber,
            identity.Model,
            fuelType,
            horsePower,
            boilerPressureBar);
    }

    public Engine CreateTurbojetEngine()
    {
        EngineIdentity identity = ReadIdentity();
        TurbineInput input = ReadTurbineInput();

        return EngineFactory.CreateTurbojetEngine(
            identity.SerialNumber,
            identity.Model,
            input.FuelType,
            input.ThrustKn);
    }

    public Engine CreateTurbofanEngine()
    {
        EngineIdentity identity = ReadIdentity();
        TurbineInput input = ReadTurbineInput();
        double bypassRatio = ConsoleInput.ReadDouble(
            "Bypass ratio: ", 0, 100);

        return EngineFactory.CreateTurbofanEngine(
            identity.SerialNumber,
            identity.Model,
            input.FuelType,
            input.ThrustKn,
            bypassRatio);
    }

    public Engine CreateTurbopropEngine()
    {
        EngineIdentity identity = ReadIdentity();
        TurbineInput input = ReadTurbineInput();
        int shaftHorsePower = ReadShaftHorsePower();

        return EngineFactory.CreateTurbopropEngine(
            identity.SerialNumber,
            identity.Model,
            input.FuelType,
            input.ThrustKn,
            shaftHorsePower);
    }

    public Engine CreateTurboshaftEngine()
    {
        EngineIdentity identity = ReadIdentity();
        TurbineInput input = ReadTurbineInput();
        int shaftHorsePower = ReadShaftHorsePower();

        return EngineFactory.CreateTurboshaftEngine(
            identity.SerialNumber,
            identity.Model,
            input.FuelType,
            input.ThrustKn,
            shaftHorsePower);
    }

    public void PrintResult(EngineCreatorResult result)
    {
        Console.WriteLine();
        Console.WriteLine(result.Message);

        if (!result.Success)
        {
            return;
        }

        foreach (Engine engine in result.Engines)
        {
            Console.WriteLine();
            PrintEngine(engine);
        }
    }

    public void PrintEngine(Engine engine)
    {
        Console.WriteLine($"Type: {engine.GetType().Name}");
        Console.WriteLine($"Serial number: {engine.SerialNumber}");
        Console.WriteLine($"Model: {engine.Model}");

        if (engine.FuelType is not null)
        {
            Console.WriteLine($"Fuel type: {engine.FuelType}");
        }

        switch (engine)
        {
            case PistonEngine piston:
                Console.WriteLine(
                    $"Horsepower: {piston.Specification.HorsePower}");
                Console.WriteLine(
                    $"Displacement: {piston.Specification.DisplacementCc} cc");
                Console.WriteLine(
                    $"Cylinders: {piston.Specification.CylinderCount}");
                break;

            case RotaryEngine rotary:
                Console.WriteLine(
                    $"Horsepower: {rotary.Specification.HorsePower}");
                Console.WriteLine(
                    $"Displacement: {rotary.Specification.DisplacementCc} cc");
                Console.WriteLine(
                    $"Rotors: {rotary.Specification.RotorCount}");
                break;

            case ElectricMotor electricMotor:
                Console.WriteLine(
                    $"Power: {electricMotor.Specification.PowerKw} kW");
                Console.WriteLine(
                    $"Voltage: {electricMotor.Specification.Voltage} V");
                break;

            case SteamEngine steam:
                Console.WriteLine(
                    $"Horsepower: {steam.Specification.HorsePower}");
                Console.WriteLine(
                    $"Boiler pressure: " +
                    $"{steam.Specification.BoilerPressureBar} bar");
                break;

            case TurbofanEngine turbofan:
                Console.WriteLine(
                    $"Thrust: {turbofan.Specification.ThrustKn} kN");
                Console.WriteLine(
                    $"Bypass ratio: {turbofan.Specification.BypassRatio}");
                break;

            case TurbopropEngine turboprop:
                Console.WriteLine(
                    $"Thrust: {turboprop.Specification.ThrustKn} kN");
                Console.WriteLine(
                    $"Shaft horsepower: " +
                    $"{turboprop.Specification.ShaftHorsePower}");
                break;

            case TurboshaftEngine turboshaft:
                Console.WriteLine(
                    $"Thrust: {turboshaft.Specification.ThrustKn} kN");
                Console.WriteLine(
                    $"Shaft horsepower: " +
                    $"{turboshaft.Specification.ShaftHorsePower}");
                break;

            case TurbojetEngine turbojet:
                Console.WriteLine(
                    $"Thrust: {turbojet.Specification.ThrustKn} kN");
                break;
        }
    }

    private static EngineIdentity ReadIdentity()
    {
        string serialNumber = ConsoleInput.ReadRequiredString(
            "Engine serial number: ");
        string model = ConsoleInput.ReadRequiredString(
            "Engine model: ");

        return new EngineIdentity(serialNumber, model);
    }

    private static PistonInput ReadPistonInput()
    {
        return new PistonInput(
            ReadFuelType(),
            ReadHorsePower(),
            ReadDisplacementCc(),
            ConsoleInput.ReadInt("Number of cylinders: ", 1, 100));
    }

    private static TurbineInput ReadTurbineInput()
    {
        return new TurbineInput(
            ReadFuelType(),
            ConsoleInput.ReadDouble(
                "Thrust in kN: ", 0.01, 1_000_000));
    }

    private static FuelType ReadFuelType()
    {
        return ConsoleInput.ReadEnum<FuelType>("Fuel type:");
    }

    private static int ReadHorsePower()
    {
        return ConsoleInput.ReadInt(
            "Horsepower: ", 1, 100_000);
    }

    private static int ReadDisplacementCc()
    {
        return ConsoleInput.ReadInt(
            "Displacement in cc: ", 1, 1_000_000);
    }

    private static int ReadShaftHorsePower()
    {
        return ConsoleInput.ReadInt(
            "Shaft horsepower: ", 1, 100_000);
    }

    private sealed record EngineIdentity(
        string SerialNumber,
        string Model);

    private sealed record PistonInput(
        FuelType FuelType,
        int HorsePower,
        int DisplacementCc,
        int CylinderCount);

    private sealed record TurbineInput(
        FuelType FuelType,
        double ThrustKn);
}
