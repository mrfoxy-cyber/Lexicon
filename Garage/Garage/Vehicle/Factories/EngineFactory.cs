using Garage.Engines;
using Garage.Engines.InternalCombustion.Turbine;
using Garage.Engines.Electric;
using Garage.Engines.ExternalCombustion;
using Garage.Engines.InternalCombustion;
using Garage.Engines.InternalCombustion.Piston;
using Garage.Engines.Specifications;

namespace Garage.Factories;

public static class EngineFactory
{
    public static InlineEngine CreateInlineEngine(
        string model,
        FuelType fuelType,
        int horsePower,
        int displacementCc,
        int cylinderCount)
    {
        return new InlineEngine
        {
            Model = model,
            Specification = new PistonEngineSpecification
            {
                FuelType = fuelType,
                HorsePower = horsePower,
                DisplacementCc = displacementCc,
                CylinderCount = cylinderCount
            }
        };
    }

    public static VEngine CreateVEngine(
        string model,
        FuelType fuelType,
        int horsePower,
        int displacementCc,
        int cylinderCount)
    {
        return new VEngine
        {
            Model = model,
            Specification = new PistonEngineSpecification
            {
                FuelType = fuelType,
                HorsePower = horsePower,
                DisplacementCc = displacementCc,
                CylinderCount = cylinderCount
            }
        };
    }

    public static FlatEngine CreateFlatEngine(
        string model,
        FuelType fuelType,
        int horsePower,
        int displacementCc,
        int cylinderCount)
    {
        return new FlatEngine
        {
            Model = model,
            Specification = new PistonEngineSpecification
            {
                FuelType = fuelType,
                HorsePower = horsePower,
                DisplacementCc = displacementCc,
                CylinderCount = cylinderCount
            }
        };
    }

    public static RotaryEngine CreateRotaryEngine(
        string model,
        FuelType fuelType,
        int horsePower,
        int displacementCc,
        int rotorCount)
    {
        return new RotaryEngine
        {
            Model = model,
            Specification = new RotaryEngineSpecification
            {
                FuelType = fuelType,
                HorsePower = horsePower,
                DisplacementCc = displacementCc,
                RotorCount = rotorCount
            }
        };
    }

    public static ElectricMotor CreateElectricMotor(
        string model,
        double powerKw,
        double voltage)
    {
        return new ElectricMotor
        {
            Model = model,
            Specification = new ElectricEngineSpecification
            {
                PowerKw = powerKw,
                Voltage = voltage
            }
        };
    }

    public static SteamEngine CreateSteamEngine(
        string model,
        FuelType fuelType,
        int horsePower,
        double boilerPressureBar)
    {
        return new SteamEngine
        {
            Model = model,
            Specification = new SteamEngineSpecification
            {
                FuelType = fuelType,
                HorsePower = horsePower,
                BoilerPressureBar = boilerPressureBar
            }
        };
    }

    public static TurbojetEngine CreateTurbojetEngine(
        string model,
        FuelType fuelType,
        double thrustKn)
    {
        return new TurbojetEngine
        {
            Model = model,
            Specification = new TurbineEngineSpecification
            {
                FuelType = fuelType,
                ThrustKn = thrustKn
            }
        };
    }

    public static TurbofanEngine CreateTurbofanEngine(
        string model,
        FuelType fuelType,
        double thrustKn,
        double bypassRatio)
    {
        return new TurbofanEngine
        {
            Model = model,
            Specification = new TurbofanEngineSpecification
            {
                FuelType = fuelType,
                ThrustKn = thrustKn,
                BypassRatio = bypassRatio
            }
        };
    }

    public static TurbopropEngine CreateTurbopropEngine(
        string model,
        FuelType fuelType,
        double thrustKn,
        int shaftHorsePower)
    {
        return new TurbopropEngine
        {
            Model = model,
            Specification = new TurbopropEngineSpecification
            {
                FuelType = fuelType,
                ThrustKn = thrustKn,
                ShaftHorsePower = shaftHorsePower
            }
        };
    }

    public static TurboshaftEngine CreateTurboshaftEngine(
        string model,
        FuelType fuelType,
        double thrustKn,
        int shaftHorsePower)
    {
        return new TurboshaftEngine
        {
            Model = model,
            Specification = new TurboshaftEngineSpecification
            {
                FuelType = fuelType,
                ThrustKn = thrustKn,
                ShaftHorsePower = shaftHorsePower
            }
        };
    }
}
