using System.Text.Json;
using System.Text.Json.Serialization;
using Garage.Factories;
using Garage.Vehicles;

namespace Garage.ConsoleApp.Data;

public static class SeedData
{
    public static List<Vehicle> LoadVehicles(string? filePath = null)
    {
        // same as if (filePath is null)
        filePath ??= Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "vehicles.json");

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


        /* fancy way to say 

                List<SeedVehicleDto>? result =
                    JsonSerializer.Deserialize<List<SeedVehicleDto>>(
                        json,
                        options);

                List<SeedVehicleDto> seedVehicles;

                if (result is null)
                {
                    seedVehicles = new List<SeedVehicleDto>();
                }
                else
                {
                    seedVehicles = result;
                }
}

        */
        List<SeedVehicleDto> seedVehicles =
            JsonSerializer.Deserialize<List<SeedVehicleDto>>(
                json,
                options) ?? [];

        return seedVehicles
            .Select(CreateVehicle)
            .ToList();
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
            Require(seed.NumberOfSeats, seed, nameof(seed.NumberOfSeats)));
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
            Require(seed.DriverSeats, seed, nameof(seed.DriverSeats)));
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
            Require(seed.DriverSeatCount, seed, nameof(seed.DriverSeatCount)));
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
            Require(seed.MaximumLoadKg, seed, nameof(seed.MaximumLoadKg)));
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
                nameof(seed.CabinCrewSeatCount)));
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
                nameof(seed.MaximumAltitudeMeters)));
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

    private static TValue Require<TValue>(
        TValue? value,
        SeedVehicleDto seed,
        string propertyName)
        where TValue : struct
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
}
