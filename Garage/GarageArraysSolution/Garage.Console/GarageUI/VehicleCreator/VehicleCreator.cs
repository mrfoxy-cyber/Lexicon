using Garage.ConsoleApp.ConsoleFramework;
using Garage.Engines;
using Garage.Vehicles;
using Garage.Vehicles.Specifications;
using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.GarageUI;

public sealed class VehicleCreator
{
    private readonly CreatedVehicleStore createdVehicles;
    private readonly VehicleActions vehicleActions = new();
    private readonly EngineCreator engineCreator = new();

    private readonly CarDraft carDraft = new();
    private readonly MotorcycleDraft motorcycleDraft = new();
    private readonly AirplaneDraft airplaneDraft = new();
    private readonly BusDraft busDraft = new();
    private readonly BoatDraft boatDraft = new();
    private readonly TruckDraft truckDraft = new();

    public VehicleCreator(CreatedVehicleStore createdVehicles)
    {
        this.createdVehicles = createdVehicles;
    }

    public void ShowMenu()
    {
        ConsoleMenu menu = new(
            title: "Vehicle creator",
            options:
            [
                new MenuOption("Create car", ShowCarMenu),
                new MenuOption("Create motorcycle", ShowMotorcycleMenu),
                new MenuOption("Create airplane", ShowAirplaneMenu),
                new MenuOption("Create bus", ShowBusMenu),
                new MenuOption("Create boat", ShowBoatMenu),
                new MenuOption("Create truck", ShowTruckMenu),
                new MenuOption(
                    () => $"List created vehicles ({createdVehicles.Vehicles.Length})",
                    ListCreatedVehicles)
            ]);

        menu.Show();
    }

    private void ShowCarMenu()
    {
        ShowDraftMenu(
            "Create car",
            carDraft,
            [
                TextOption("VIN", () => carDraft.Vin, value => carDraft.Vin = value),
                new MenuOption(
                    () => $"Body type: {ValueText(carDraft.BodyType)}",
                    () => carDraft.BodyType = (BodyType)ConsoleInput.ReadEnum(
                        typeof(BodyType), "Body type:")),
                IntOption("Doors", () => carDraft.NumberOfDoors, value => carDraft.NumberOfDoors = value, 1, 20),
                IntOption("Seats", () => carDraft.NumberOfSeats, value => carDraft.NumberOfSeats = value, 1, 100)
            ],
            () => CreateVehicle(
                carDraft,
                () => vehicleActions.CreateCar(carDraft)));
    }

    private void ShowMotorcycleMenu()
    {
        ShowDraftMenu(
            "Create motorcycle",
            motorcycleDraft,
            [
                TextOption("VIN", () => motorcycleDraft.Vin, value => motorcycleDraft.Vin = value),
                new MenuOption(
                    () => $"Motorcycle type: {ValueText(motorcycleDraft.MotorcycleType)}",
                    () => motorcycleDraft.MotorcycleType = (MotorcycleType)ConsoleInput.ReadEnum(
                        typeof(MotorcycleType), "Motorcycle type:")),
                BoolOption("Has sidecar", () => motorcycleDraft.HasSidecar, value => motorcycleDraft.HasSidecar = value),
                IntOption("Wheels", () => motorcycleDraft.NumberOfWheels, value => motorcycleDraft.NumberOfWheels = value, 1, 4),
                IntOption("Passenger capacity", () => motorcycleDraft.PassengerCapacity, value => motorcycleDraft.PassengerCapacity = value, 0, 10),
                IntOption("Driver seats", () => motorcycleDraft.DriverSeats, value => motorcycleDraft.DriverSeats = value, 1, 2)
            ],
            () => CreateVehicle(
                motorcycleDraft,
                () => vehicleActions.CreateMotorcycle(motorcycleDraft)));
    }

    private void ShowAirplaneMenu()
    {
        ShowDraftMenu(
            "Create airplane",
            airplaneDraft,
            [
                TextOption("Airframe serial number", () => airplaneDraft.AirframeSerialNumber, value => airplaneDraft.AirframeSerialNumber = value),
                IntOption("Wheels", () => airplaneDraft.NumberOfWheels, value => airplaneDraft.NumberOfWheels = value, 0, VehicleSpecification.MaximumNumberOfWheels),
                IntOption("Passenger seats", () => airplaneDraft.PassengerSeats, value => airplaneDraft.PassengerSeats = value, 0, 1_000),
                IntOption("Crew seats", () => airplaneDraft.CrewSeats, value => airplaneDraft.CrewSeats = value, 0, 100),
                IntOption("Pilot seats", () => airplaneDraft.PilotSeats, value => airplaneDraft.PilotSeats = value, 0, 10),
                BoolOption("Has dual controls", () => airplaneDraft.HasDualControls, value => airplaneDraft.HasDualControls = value),
                DoubleOption("Maximum altitude", "meters", () => airplaneDraft.MaximumAltitudeMeters, value => airplaneDraft.MaximumAltitudeMeters = value, 0, 100_000)
            ],
            () => CreateVehicle(
                airplaneDraft,
                () => vehicleActions.CreateAirplane(airplaneDraft)),
            middleDimensionName: "Wingspan");
    }

    private void ShowBusMenu()
    {
        ShowDraftMenu(
            "Create bus",
            busDraft,
            [
                TextOption("VIN", () => busDraft.Vin, value => busDraft.Vin = value),
                IntOption("Doors", () => busDraft.NumberOfDoors, value => busDraft.NumberOfDoors = value, 1, 20),
                IntOption("Wheels", () => busDraft.NumberOfWheels, value => busDraft.NumberOfWheels = value, 2, VehicleSpecification.MaximumNumberOfWheels),
                IntOption("Passenger seats", () => busDraft.PassengerSeats, value => busDraft.PassengerSeats = value, 0, 500),
                IntOption("Standing capacity", () => busDraft.StandingPassengerCapacity, value => busDraft.StandingPassengerCapacity = value, 0, 500),
                IntOption("Driver seats", () => busDraft.DriverSeats, value => busDraft.DriverSeats = value, 0, 10)
            ],
            () => CreateVehicle(
                busDraft,
                () => vehicleActions.CreateBus(busDraft)));
    }

    private void ShowBoatMenu()
    {
        ShowDraftMenu(
            "Create boat",
            boatDraft,
            [
                TextOption("HIN", () => boatDraft.Hin, value => boatDraft.Hin = value),
                DoubleOption("Draft", "meters", () => boatDraft.DraftMeters, value => boatDraft.DraftMeters = value, 0, 100),
                new MenuOption(
                    () => $"Boat type: {ValueText(boatDraft.BoatType)}",
                    () => boatDraft.BoatType = (BoatType)ConsoleInput.ReadEnum(
                        typeof(BoatType), "Boat type:")),
                IntOption("Passenger capacity", () => boatDraft.PassengerCapacity, value => boatDraft.PassengerCapacity = value, 0, 10_000),
                IntOption("Driver seats", () => boatDraft.DriverSeats, value => boatDraft.DriverSeats = value, 1, 100),
                BoolOption("Has cabin", () => boatDraft.HasCabin, value => boatDraft.HasCabin = value),
                IntOption("Cabin passenger seats", () => boatDraft.CabinPassengerSeats, value => boatDraft.CabinPassengerSeats = value, 0, 10_000),
                IntOption("Cabin crew seats", () => boatDraft.CabinCrewSeats, value => boatDraft.CabinCrewSeats = value, 0, 1_000)
            ],
            () => CreateVehicle(
                boatDraft,
                () => vehicleActions.CreateBoat(boatDraft)));
    }

    private void ShowTruckMenu()
    {
        ShowDraftMenu(
            "Create truck",
            truckDraft,
            [
                TextOption("VIN", () => truckDraft.Vin, value => truckDraft.Vin = value),
                IntOption("Wheels", () => truckDraft.NumberOfWheels, value => truckDraft.NumberOfWheels = value, 2, VehicleSpecification.MaximumNumberOfWheels),
                IntOption("Axles", () => truckDraft.NumberOfAxles, value => truckDraft.NumberOfAxles = value, 1, 20),
                DoubleOption("Maximum load", "kg", () => truckDraft.MaximumLoadKg, value => truckDraft.MaximumLoadKg = value, 0, 1_000_000)
            ],
            () => CreateVehicle(
                truckDraft,
                () => vehicleActions.CreateTruck(truckDraft)));
    }

    private void ShowDraftMenu(
        string title,
        VehicleDraft draft,
        MenuOption[] typeOptions,
        Action createVehicle,
        string middleDimensionName = "Width")
    {
        MenuOption[] options =
        [
            .. CreateCommonOptions(draft, middleDimensionName),
            .. typeOptions,
            new MenuOption(
                () => $"Engines: {draft.EnginesText}",
                () => AddEngine(draft)),
            new MenuOption(
                "List draft engines",
                () => ListDraftEngines(draft)),
            new MenuOption(
                "Remove draft engine",
                () => RemoveDraftEngine(draft)),
            new MenuOption("Create vehicle", createVehicle),
            new MenuOption("Clear all fields", draft.Clear)
        ];

        new ConsoleMenu(title, options).Show();
    }

    private static MenuOption[] CreateCommonOptions(
        VehicleDraft draft,
        string middleDimensionName)
    {
        return
        [
            TextOption("Registration", () => draft.RegistrationNumber, value => draft.RegistrationNumber = value),
            TextOption("Manufacturer", () => draft.Manufacturer, value => draft.Manufacturer = value),
            TextOption("Model", () => draft.Model, value => draft.Model = value),
            IntOption("Year", () => draft.ManufacturingYear, value => draft.ManufacturingYear = value, 1886, DateTime.Now.Year + 1),
            new MenuOption(
                () => $"Color: {draft.ColorText}",
                () => draft.Color = (VehicleColor)ConsoleInput.ReadEnum(
                    typeof(VehicleColor), "Color:")),
            DoubleOption("Weight", "kg", () => draft.WeightKg, value => draft.WeightKg = value, 0.1, 1_000_000),
            new MenuOption(
                () => $"Dimensions: {draft.DimensionsText}",
                () =>
                {
                    draft.LengthMeters = ReadMeasurement("Length in meters: ");
                    draft.WidthMeters = ReadMeasurement($"{middleDimensionName} in meters: ");
                    draft.HeightMeters = ReadMeasurement("Height in meters: ");
                })
        ];
    }

    private void CreateVehicle(
        VehicleDraft draft,
        VehicleFactory factory)
    {
        string[] missing = draft.GetMissingFields();

        if (missing.Length > 0)
        {
            Console.WriteLine(
                VehicleCreatorResult.Failed(
                    "Complete these vehicle fields: " +
                    string.Join(", ", missing)).Message);
            ConsoleInput.Pause();
            return;
        }

        Vehicle vehicle;

        //Domain magic happens here, we catch error from domain here

        try
        {
            vehicle = factory();
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine(
                VehicleCreatorResult.Failed(
                    exception.Message).Message);
            ConsoleInput.Pause();
            return;
        }

        createdVehicles.Add(vehicle);
        Console.WriteLine(VehicleCreatorResult.Completed(vehicle).Message);
        draft.Clear();
        ConsoleInput.Pause();
    }

    private void AddEngine(VehicleDraft draft)
    {
        engineCreator.ShowCreateEngineMenu(
            engine => draft.AddEngines([engine]));
    }

    private static void ListDraftEngines(VehicleDraft draft)
    {
        Engine[] engines = draft.Engines;

        if (engines.Length == 0)
        {
            Console.WriteLine("The vehicle draft has no engines.");
            ConsoleInput.Pause();
            return;
        }

        for (int index = 0; index < engines.Length; index++)
        {
            Console.WriteLine($"Engine {index + 1}");
            EngineActions.PrintEngine(engines[index]);
            Console.WriteLine();
        }

        ConsoleInput.Pause();
    }

    private static void RemoveDraftEngine(VehicleDraft draft)
    {
        Engine[] engines = draft.Engines;

        if (engines.Length == 0)
        {
            Console.WriteLine("The vehicle draft has no engines.");
            ConsoleInput.Pause();
            return;
        }

        for (int index = 0; index < engines.Length; index++)
        {
            Console.WriteLine(
                $"{index + 1}. {engines[index].GetType().Name} " +
                $"{engines[index].Model} ({engines[index].SerialNumber})");
        }

        int choice = ConsoleInput.ReadInt(
            "Engine to remove: ", 1, engines.Length);
        draft.RemoveEngine(engines[choice - 1]);
    }

    private void ListCreatedVehicles()
    {
        Vehicle[] vehicles = createdVehicles.Vehicles;

        if (vehicles.Length == 0)
        {
            Console.WriteLine("There are no created vehicles.");
            ConsoleInput.Pause();
            return;
        }

        for (int index = 0; index < vehicles.Length; index++)
        {
            Vehicle vehicle = vehicles[index];
            Console.WriteLine(
                $"{index + 1}. {vehicle.GetType().Name}, " +
                $"{vehicle.Manufacturer} {vehicle.Model}, " +
                $"{vehicle.RegistrationNumber}");
        }

        ConsoleInput.Pause();
    }

    private static MenuOption TextOption(
        string label,
        TextValueGetter getter,
        TextValueSetter setter)
    {
        return new MenuOption(
            () => $"{label}: {getter() ?? "Not set"}",
            () => setter(ConsoleInput.ReadRequiredString($"{label}: ")));
    }

    private static MenuOption IntOption(
        string label,
        IntValueGetter getter,
        IntValueSetter setter,
        int minimum,
        int maximum)
    {
        return new MenuOption(
            () => $"{label}: {ValueText(getter())}",
            () => setter(ConsoleInput.ReadInt(
                $"{label}: ", minimum, maximum)));
    }

    private static MenuOption DoubleOption(
        string label,
        string unit,
        DoubleValueGetter getter,
        DoubleValueSetter setter,
        double minimum,
        double maximum)
    {
        return new MenuOption(
            () => $"{label}: {ValueText(getter(), unit)}",
            () => setter(ConsoleInput.ReadDouble(
                $"{label} in {unit}: ", minimum, maximum)));
    }

    private static MenuOption BoolOption(
        string label,
        BoolValueGetter getter,
        BoolValueSetter setter)
    {
        return new MenuOption(
            () => $"{label}: {ValueText(getter())}",
            () => setter(ConsoleInput.ReadBoolean(label)));
    }

    private static string ValueText(
        object? value,
        string? unit = null)
    {
        return value is null
            ? "Not set"
            : unit is null
                ? value.ToString() ?? "Not set"
                : $"{value} {unit}";
    }

    private static double ReadMeasurement(string prompt)
    {
        return ConsoleInput.ReadDouble(
            prompt,
            0.01,
            VehicleSpecification.MaximumLengthMeters);
    }
}
