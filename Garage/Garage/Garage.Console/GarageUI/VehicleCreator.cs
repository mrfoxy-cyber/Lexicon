using Garage.ConsoleApp.ConsoleFramework;
using Garage.Factories;
using Garage.Vehicles;
using Garage.Vehicles.Specifications;
using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.GarageUI;

public sealed class VehicleCreator
{
    public Car CreateCar()
    {
        VehicleInput vehicle = ReadVehicleInput();
        DimensionsInput dimensions = ReadDimensions();

        string vin = ConsoleInput.ReadRequiredString("VIN: ");
        BodyType bodyType =
            ConsoleInput.ReadEnum<BodyType>("Body type:");
        int numberOfDoors = ConsoleInput.ReadInt(
            "Number of doors: ", 1, 20);
        int numberOfSeats = ConsoleInput.ReadInt(
            "Number of seats: ", 1, 100);

        return CarFactory.CreateCar(
            vehicle.RegistrationNumber,
            vehicle.Manufacturer,
            vehicle.Model,
            vehicle.ManufacturingYear,
            vehicle.Color,
            vehicle.WeightKg,
            dimensions.LengthMeters,
            dimensions.WidthMeters,
            dimensions.HeightMeters,
            vin,
            bodyType,
            numberOfDoors,
            numberOfSeats);
    }

    public Motorcycle CreateMotorcycle()
    {
        VehicleInput vehicle = ReadVehicleInput();
        DimensionsInput dimensions = ReadDimensions();

        string vin = ConsoleInput.ReadRequiredString("VIN: ");
        MotorcycleType motorcycleType =
            ConsoleInput.ReadEnum<MotorcycleType>("Motorcycle type:");
        bool hasSidecar = ConsoleInput.ReadBoolean("Has sidecar");
        int numberOfWheels = ConsoleInput.ReadInt(
            "Number of wheels: ", 1, 4);
        int passengerCapacity = ConsoleInput.ReadInt(
            "Passenger capacity: ", 0, 10);
        int driverSeats = ConsoleInput.ReadInt(
            "Driver seats: ", 1, 2);

        return MotorcycleFactory.CreateMotorcycle(
            vehicle.RegistrationNumber,
            vehicle.Manufacturer,
            vehicle.Model,
            vehicle.ManufacturingYear,
            vehicle.Color,
            vehicle.WeightKg,
            dimensions.LengthMeters,
            dimensions.WidthMeters,
            dimensions.HeightMeters,
            vin,
            motorcycleType,
            hasSidecar,
            numberOfWheels,
            passengerCapacity,
            driverSeats);
    }

    public Airplane CreateAirplane()
    {
        VehicleInput vehicle = ReadVehicleInput();

        double lengthMeters = ReadMeasurement("Length in meters: ");
        double wingspanMeters = ReadMeasurement("Wingspan in meters: ");
        double heightMeters = ReadMeasurement("Height in meters: ");
        string serialNumber = ConsoleInput.ReadRequiredString(
            "Airframe serial number: ");
        int numberOfWheels = ConsoleInput.ReadInt(
            "Number of wheels: ",
            0,
            VehicleSpecification.MaximumNumberOfWheels);
        int passengerSeats = ConsoleInput.ReadInt(
            "Passenger seats: ", 0, 1_000);
        int crewSeats = ConsoleInput.ReadInt(
            "Crew seats: ", 0, 100);
        int pilotSeats = ConsoleInput.ReadInt(
            "Pilot seats: ", 0, 10);
        bool hasDualControls = ConsoleInput.ReadBoolean(
            "Has dual controls");
        double maximumAltitude = ConsoleInput.ReadDouble(
            "Maximum altitude in meters: ", 0, 100_000);

        return AirplaneFactory.CreateAirplane(
            vehicle.RegistrationNumber,
            vehicle.Manufacturer,
            vehicle.Model,
            vehicle.ManufacturingYear,
            vehicle.Color,
            vehicle.WeightKg,
            numberOfWheels,
            lengthMeters,
            wingspanMeters,
            heightMeters,
            serialNumber,
            passengerSeats,
            crewSeats,
            pilotSeats,
            hasDualControls,
            maximumAltitude);
    }

    public Bus CreateBus()
    {
        VehicleInput vehicle = ReadVehicleInput();
        DimensionsInput dimensions = ReadDimensions();

        string vin = ConsoleInput.ReadRequiredString("VIN: ");
        int numberOfDoors = ConsoleInput.ReadInt(
            "Number of doors: ", 1, 20);
        int numberOfWheels = ConsoleInput.ReadInt(
            "Number of wheels: ",
            2,
            VehicleSpecification.MaximumNumberOfWheels);
        int passengerSeats = ConsoleInput.ReadInt(
            "Passenger seats: ", 0, 500);
        int standingCapacity = ConsoleInput.ReadInt(
            "Standing passenger capacity: ", 0, 500);
        int driverSeats = ConsoleInput.ReadInt(
            "Driver seats: ", 1, 10);

        return BusFactory.CreateBus(
            vehicle.RegistrationNumber,
            vehicle.Manufacturer,
            vehicle.Model,
            vehicle.ManufacturingYear,
            vehicle.Color,
            vehicle.WeightKg,
            dimensions.LengthMeters,
            dimensions.WidthMeters,
            dimensions.HeightMeters,
            vin,
            numberOfDoors,
            numberOfWheels,
            passengerSeats,
            standingCapacity,
            driverSeats);
    }

    public Boat CreateBoat()
    {
        VehicleInput vehicle = ReadVehicleInput();
        DimensionsInput dimensions = ReadDimensions();

        string hin = ConsoleInput.ReadRequiredString("HIN: ");
        double draftMeters = ConsoleInput.ReadDouble(
            "Draft in meters: ", 0, 100);
        BoatType boatType =
            ConsoleInput.ReadEnum<BoatType>("Boat type:");
        int passengerCapacity = ConsoleInput.ReadInt(
            "Passenger capacity: ", 0, 10_000);
        int driverSeats = ConsoleInput.ReadInt(
            "Driver seats: ", 1, 100);
        bool hasCabin = ConsoleInput.ReadBoolean("Has cabin");

        int cabinPassengerSeats = hasCabin
            ? ConsoleInput.ReadInt("Cabin passenger seats: ", 0, 10_000)
            : 0;

        int cabinCrewSeats = hasCabin
            ? ConsoleInput.ReadInt("Cabin crew seats: ", 0, 1_000)
            : 0;

        return BoatFactory.CreateBoat(
            vehicle.RegistrationNumber,
            vehicle.Manufacturer,
            vehicle.Model,
            vehicle.ManufacturingYear,
            vehicle.Color,
            vehicle.WeightKg,
            dimensions.LengthMeters,
            dimensions.WidthMeters,
            dimensions.HeightMeters,
            hin,
            draftMeters,
            boatType,
            passengerCapacity,
            driverSeats,
            hasCabin,
            cabinPassengerSeats,
            cabinCrewSeats);
    }

    public Truck CreateTruck()
    {
        VehicleInput vehicle = ReadVehicleInput();
        DimensionsInput dimensions = ReadDimensions();

        string vin = ConsoleInput.ReadRequiredString("VIN: ");
        int numberOfWheels = ConsoleInput.ReadInt(
            "Number of wheels: ",
            2,
            VehicleSpecification.MaximumNumberOfWheels);
        int numberOfAxles = ConsoleInput.ReadInt(
            "Number of axles: ", 1, 20);
        double maximumLoadKg = ConsoleInput.ReadDouble(
            "Maximum load in kg: ", 0, 1_000_000);

        return TruckFactory.CreateTruck(
            vehicle.RegistrationNumber,
            vehicle.Manufacturer,
            vehicle.Model,
            vehicle.ManufacturingYear,
            vehicle.Color,
            vehicle.WeightKg,
            dimensions.LengthMeters,
            dimensions.WidthMeters,
            dimensions.HeightMeters,
            vin,
            numberOfWheels,
            numberOfAxles,
            maximumLoadKg);
    }

    private static VehicleInput ReadVehicleInput()
    {
        string registrationNumber =
            ConsoleInput.ReadRequiredString("Registration number: ");
        string manufacturer =
            ConsoleInput.ReadRequiredString("Manufacturer: ");
        string model =
            ConsoleInput.ReadRequiredString("Model: ");
        int manufacturingYear = ConsoleInput.ReadInt(
            "Manufacturing year: ",
            minimum: 1886,
            maximum: DateTime.Now.Year + 1);
        VehicleColor color =
            ConsoleInput.ReadEnum<VehicleColor>("Color:");
        double weightKg = ConsoleInput.ReadDouble(
            "Weight in kg: ", 0.1, 1_000_000);

        return new VehicleInput(
            registrationNumber,
            manufacturer,
            model,
            manufacturingYear,
            color,
            weightKg);
    }

    private static DimensionsInput ReadDimensions()
    {
        return new DimensionsInput(
            ReadMeasurement("Length in meters: "),
            ReadMeasurement("Width in meters: "),
            ReadMeasurement("Height in meters: "));
    }

    private static double ReadMeasurement(string prompt)
    {
        return ConsoleInput.ReadDouble(
            prompt,
            0.01,
            VehicleSpecification.MaximumLengthMeters);
    }

    private sealed record VehicleInput(
        string RegistrationNumber,
        string Manufacturer,
        string Model,
        int ManufacturingYear,
        VehicleColor Color,
        double WeightKg);

    private sealed record DimensionsInput(
        double LengthMeters,
        double WidthMeters,
        double HeightMeters);
}
