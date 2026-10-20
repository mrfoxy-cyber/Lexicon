using System.Text.Json;
using System.Text.Json.Serialization;
using Garage.Engines;
using Garage.Factories;
using Garage.Vehicles;
using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.Data;

public static class SeedData
{
    public static Vehicle[] LoadVehicles(string? filePath = null)
    {
        filePath ??= "vehicles.json";

        if (!Path.IsPathRooted(filePath))
        {
            filePath = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                filePath);
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "The seed vehicle file could not be found.",
                filePath);
        }

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        string json = File.ReadAllText(filePath);


        SeedVehicleDto[] seedVehicles =
            (SeedVehicleDto[]?)JsonSerializer.Deserialize(
                json,
                typeof(SeedVehicleDto[]),
                options) ?? [];

        Vehicle[] vehicles =
    new Vehicle[seedVehicles.Length];

        for (int i = 0; i < seedVehicles.Length; i++)
        {
            vehicles[i] = CreateVehicle(seedVehicles[i]);
        }

        return vehicles;
    }

    private static Vehicle CreateVehicle(SeedVehicleDto seed)
    {
        return seed.Type.Trim().ToLowerInvariant() switch
        {
            "car" => CreateCar(seed),
            "motorcycle" => CreateMotorcycle(seed),
            "bus" => CreateBus(seed),
            "truck" => CreateTruck(seed),
            "boat" => CreateBoat(seed),
            "airplane" => CreateAirplane(seed),

            _ => throw new InvalidDataException(
                $"Unknown vehicle type '{seed.Type}' for " +
                $"registration '{seed.RegistrationNumber}'.")
        };
    }

    private static Car CreateCar(SeedVehicleDto seed)
    {
        return CarFactory.CreateCar(
            seed.RegistrationNumber,
            seed.Manufacturer,
            seed.Model,
            seed.ManufacturingYear,
            seed.Color,
            seed.WeightKg,
            seed.LengthMeters,
            seed.WidthMeters,
            seed.HeightMeters,
            Require(seed.Vin, seed, nameof(seed.Vin)),
            Require(seed.BodyType, seed, nameof(seed.BodyType)),
            Require(seed.NumberOfDoors, seed, nameof(seed.NumberOfDoors)),
            Require(seed.NumberOfSeats, seed, nameof(seed.NumberOfSeats)),
            CreateEngines(seed));
    }

    private static Motorcycle CreateMotorcycle(SeedVehicleDto seed)
    {
        return MotorcycleFactory.CreateMotorcycle(
            seed.RegistrationNumber,
            seed.Manufacturer,
            seed.Model,
            seed.ManufacturingYear,
            seed.Color,
            seed.WeightKg,
            seed.LengthMeters,
            seed.WidthMeters,
            seed.HeightMeters,
            Require(seed.Vin, seed, nameof(seed.Vin)),
            Require(seed.MotorcycleType, seed, nameof(seed.MotorcycleType)),
            Require(seed.HasSidecar, seed, nameof(seed.HasSidecar)),
            Require(seed.NumberOfWheels, seed, nameof(seed.NumberOfWheels)),
            Require(seed.PassengerCapacity, seed, nameof(seed.PassengerCapacity)),
            Require(seed.DriverSeats, seed, nameof(seed.DriverSeats)),
            CreateEngines(seed));
    }

    private static Bus CreateBus(SeedVehicleDto seed)
    {
        return BusFactory.CreateBus(
            seed.RegistrationNumber,
            seed.Manufacturer,
            seed.Model,
            seed.ManufacturingYear,
            seed.Color,
            seed.WeightKg,
            seed.LengthMeters,
            seed.WidthMeters,
            seed.HeightMeters,
            Require(seed.Vin, seed, nameof(seed.Vin)),
            Require(seed.NumberOfDoors, seed, nameof(seed.NumberOfDoors)),
            Require(seed.NumberOfWheels, seed, nameof(seed.NumberOfWheels)),
            Require(seed.PassengerSeatCount, seed, nameof(seed.PassengerSeatCount)),
            Require(
                seed.StandingPassengerCapacity,
                seed,
                nameof(seed.StandingPassengerCapacity)),
            Require(seed.DriverSeatCount, seed, nameof(seed.DriverSeatCount)),
            CreateEngines(seed));
    }

    private static Truck CreateTruck(SeedVehicleDto seed)
    {
        return TruckFactory.CreateTruck(
            seed.RegistrationNumber,
            seed.Manufacturer,
            seed.Model,
            seed.ManufacturingYear,
            seed.Color,
            seed.WeightKg,
            seed.LengthMeters,
            seed.WidthMeters,
            seed.HeightMeters,
            Require(seed.Vin, seed, nameof(seed.Vin)),
            Require(seed.NumberOfWheels, seed, nameof(seed.NumberOfWheels)),
            Require(seed.NumberOfAxles, seed, nameof(seed.NumberOfAxles)),
            Require(seed.MaximumLoadKg, seed, nameof(seed.MaximumLoadKg)),
            CreateEngines(seed));
    }

    private static Boat CreateBoat(SeedVehicleDto seed)
    {
        return BoatFactory.CreateBoat(
            seed.RegistrationNumber,
            seed.Manufacturer,
            seed.Model,
            seed.ManufacturingYear,
            seed.Color,
            seed.WeightKg,
            seed.LengthMeters,
            seed.WidthMeters,
            seed.HeightMeters,
            Require(seed.Hin, seed, nameof(seed.Hin)),
            Require(seed.DraftMeters, seed, nameof(seed.DraftMeters)),
            Require(seed.BoatType, seed, nameof(seed.BoatType)),
            Require(seed.PassengerCapacity, seed, nameof(seed.PassengerCapacity)),
            Require(seed.DriverSeats, seed, nameof(seed.DriverSeats)),
            Require(seed.HasCabin, seed, nameof(seed.HasCabin)),
            Require(
                seed.CabinPassengerSeatCount,
                seed,
                nameof(seed.CabinPassengerSeatCount)),
            Require(
                seed.CabinCrewSeatCount,
                seed,
                nameof(seed.CabinCrewSeatCount)),
            CreateEngines(seed));
    }

    private static Airplane CreateAirplane(SeedVehicleDto seed)
    {
        return AirplaneFactory.CreateAirplane(
            seed.RegistrationNumber,
            seed.Manufacturer,
            seed.Model,
            seed.ManufacturingYear,
            seed.Color,
            seed.WeightKg,
            Require(seed.NumberOfWheels, seed, nameof(seed.NumberOfWheels)),
            seed.LengthMeters,
            Require(seed.WingspanMeters, seed, nameof(seed.WingspanMeters)),
            seed.HeightMeters,
            Require(
                seed.AirframeSerialNumber,
                seed,
                nameof(seed.AirframeSerialNumber)),
            Require(seed.PassengerSeatCount, seed, nameof(seed.PassengerSeatCount)),
            Require(seed.CrewSeatCount, seed, nameof(seed.CrewSeatCount)),
            Require(seed.PilotSeatCount, seed, nameof(seed.PilotSeatCount)),
            Require(seed.HasDualControls, seed, nameof(seed.HasDualControls)),
            Require(
                seed.MaximumAltitudeMeters,
                seed,
                nameof(seed.MaximumAltitudeMeters)),
            CreateEngines(seed));
    }

    private static Engine[] CreateEngines(SeedVehicleDto vehicle)
    {
        Engine[] engines = new Engine[vehicle.Engines.Length];

        for (int i = 0; i < vehicle.Engines.Length; i++)
        {
            engines[i] = CreateEngine(vehicle.Engines[i]);
        }

        return engines;
    }

    private static Engine CreateEngine(SeedEngineDto seed)
    {
        return seed.Type.Trim().ToLowerInvariant() switch
        {
            "inline" => EngineFactory.CreateInlineEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.HorsePower, seed, nameof(seed.HorsePower)),
                Require(seed.DisplacementCc, seed, nameof(seed.DisplacementCc)),
                Require(seed.CylinderCount, seed, nameof(seed.CylinderCount))),

            "vengine" => EngineFactory.CreateVEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.HorsePower, seed, nameof(seed.HorsePower)),
                Require(seed.DisplacementCc, seed, nameof(seed.DisplacementCc)),
                Require(seed.CylinderCount, seed, nameof(seed.CylinderCount))),

            "flat" => EngineFactory.CreateFlatEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.HorsePower, seed, nameof(seed.HorsePower)),
                Require(seed.DisplacementCc, seed, nameof(seed.DisplacementCc)),
                Require(seed.CylinderCount, seed, nameof(seed.CylinderCount))),

            "rotary" => EngineFactory.CreateRotaryEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.HorsePower, seed, nameof(seed.HorsePower)),
                Require(seed.DisplacementCc, seed, nameof(seed.DisplacementCc)),
                Require(seed.RotorCount, seed, nameof(seed.RotorCount))),

            "electricmotor" => EngineFactory.CreateElectricMotor(
                seed.SerialNumber,
                seed.Model,
                Require(seed.PowerKw, seed, nameof(seed.PowerKw)),
                Require(seed.Voltage, seed, nameof(seed.Voltage))),

            "steam" => EngineFactory.CreateSteamEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.HorsePower, seed, nameof(seed.HorsePower)),
                Require(
                    seed.BoilerPressureBar,
                    seed,
                    nameof(seed.BoilerPressureBar))),

            "turbojet" => EngineFactory.CreateTurbojetEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.ThrustKn, seed, nameof(seed.ThrustKn))),

            "turbofan" => EngineFactory.CreateTurbofanEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.ThrustKn, seed, nameof(seed.ThrustKn)),
                Require(seed.BypassRatio, seed, nameof(seed.BypassRatio))),

            "turboprop" => EngineFactory.CreateTurbopropEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.ThrustKn, seed, nameof(seed.ThrustKn)),
                Require(
                    seed.ShaftHorsePower,
                    seed,
                    nameof(seed.ShaftHorsePower))),

            "turboshaft" => EngineFactory.CreateTurboshaftEngine(
                seed.SerialNumber,
                seed.Model,
                Require(seed.FuelType, seed, nameof(seed.FuelType)),
                Require(seed.ThrustKn, seed, nameof(seed.ThrustKn)),
                Require(
                    seed.ShaftHorsePower,
                    seed,
                    nameof(seed.ShaftHorsePower))),

            _ => throw new InvalidDataException(
                $"Unknown engine type '{seed.Type}' for " +
                $"serial number '{seed.SerialNumber}'.")
        };
    }

    private static string Require(
        string? value,
        SeedVehicleDto seed,
        string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        throw MissingProperty(seed, propertyName);
    }

    private static int Require(
        int? value,
        SeedVehicleDto seed,
        string propertyName)
    {
        return value ?? throw MissingProperty(seed, propertyName);
    }

    private static double Require(
        double? value,
        SeedVehicleDto seed,
        string propertyName)
    {
        return value ?? throw MissingProperty(seed, propertyName);
    }

    private static bool Require(
        bool? value,
        SeedVehicleDto seed,
        string propertyName)
    {
        return value ?? throw MissingProperty(seed, propertyName);
    }

    private static BodyType Require(
        BodyType? value,
        SeedVehicleDto seed,
        string propertyName)
    {
        return value ?? throw MissingProperty(seed, propertyName);
    }

    private static MotorcycleType Require(
        MotorcycleType? value,
        SeedVehicleDto seed,
        string propertyName)
    {
        return value ?? throw MissingProperty(seed, propertyName);
    }

    private static BoatType Require(
        BoatType? value,
        SeedVehicleDto seed,
        string propertyName)
    {
        return value ?? throw MissingProperty(seed, propertyName);
    }

    private static InvalidDataException MissingProperty(
        SeedVehicleDto seed,
        string propertyName)
    {
        return new InvalidDataException(
            $"Vehicle '{seed.RegistrationNumber}' of type " +
            $"'{seed.Type}' is missing '{propertyName}'.");
    }

    private static int Require(
        int? value,
        SeedEngineDto seed,
        string propertyName)
    {
        return value ?? throw new InvalidDataException(
            $"Engine '{seed.SerialNumber}' of type " +
            $"'{seed.Type}' is missing '{propertyName}'.");
    }

    private static double Require(
        double? value,
        SeedEngineDto seed,
        string propertyName)
    {
        return value ?? throw new InvalidDataException(
            $"Engine '{seed.SerialNumber}' of type " +
            $"'{seed.Type}' is missing '{propertyName}'.");
    }

    private static FuelType Require(
        FuelType? value,
        SeedEngineDto seed,
        string propertyName)
    {
        return value ?? throw new InvalidDataException(
            $"Engine '{seed.SerialNumber}' of type " +
            $"'{seed.Type}' is missing '{propertyName}'.");
    }
}
