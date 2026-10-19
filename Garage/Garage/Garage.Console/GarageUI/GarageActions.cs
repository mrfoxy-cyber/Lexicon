using Garage.ConsoleApp.ConsoleFramework;
using Garage.Results;
using Garage.Vehicles;
using VehicleGarage = Garage.Parking.Garage<Garage.Vehicles.Vehicle>;

namespace Garage.ConsoleApp.GarageUI;

public sealed class GarageActions
{
    private readonly VehicleGarage garage;
    private readonly VehicleCreator vehicleCreator = new();

    public GarageActions(VehicleGarage garage)
    {
        this.garage = garage;
    }

    public void ParkVehicle()
    {
        if (garage.AvailableSpaces == 0)
        {
            Console.WriteLine("The garage is full.");
            ConsoleInput.Pause();
            return;
        }

        ConsoleMenu vehicleTypeMenu = new(
            title: "Choose vehicle type",
            options:
            [
                new MenuOption(
                    "Car",
                    () => Park(vehicleCreator.CreateCar())),

                new MenuOption(
                    "Motorcycle",
                    () => Park(vehicleCreator.CreateMotorcycle())),

                new MenuOption(
                    "Airplane",
                    () => Park(vehicleCreator.CreateAirplane())),

                new MenuOption(
                    "Bus",
                    () => Park(vehicleCreator.CreateBus())),

                new MenuOption(
                    "Boat",
                    () => Park(vehicleCreator.CreateBoat())),

                new MenuOption(
                    "Truck",
                    () => Park(vehicleCreator.CreateTruck()))
            ]);

        vehicleTypeMenu.Show();
    }

    public void ParkVehicleAtSpot()
    {
        if (garage.AvailableSpaces == 0)
        {
            Console.WriteLine("The garage is full.");
            ConsoleInput.Pause();
            return;
        }

        int spot = ConsoleInput.ReadInt(
            "Choose parking spot: ",
            minimum: 1,
            maximum: garage.Capacity);

        ConsoleMenu vehicleTypeMenu = new(
            title: $"Choose vehicle for spot {spot}",
            options:
            [
                new MenuOption(
                "Car",
                () => Park(vehicleCreator.CreateCar(), spot)),

            new MenuOption(
                "Motorcycle",
                () => Park(vehicleCreator.CreateMotorcycle(), spot)),

            new MenuOption(
                "Airplane",
                () => Park(vehicleCreator.CreateAirplane(), spot)),

            new MenuOption(
                "Bus",
                () => Park(vehicleCreator.CreateBus(), spot)),

            new MenuOption(
                "Boat",
                () => Park(vehicleCreator.CreateBoat(), spot)),

            new MenuOption(
                "Truck",
                () => Park(vehicleCreator.CreateTruck(), spot))
            ]);

        vehicleTypeMenu.Show();
    }

    public void ListParkedVehicles()
    {
        var parkedVehicles = garage.ParkedVehicles;

        if (parkedVehicles.Length == 0)
        {
            Console.WriteLine("The garage is empty.");
            ConsoleInput.Pause();
            return;
        }

        foreach (var parkedVehicle in parkedVehicles)
        {
            Console.WriteLine(
                $"Spot {parkedVehicle.SpotNumber}: " +
                $"{parkedVehicle.Vehicle.GetType().Name}, " +
                $"{parkedVehicle.Vehicle.RegistrationNumber}, " +
                $"{parkedVehicle.Vehicle.Manufacturer} " +
                $"{parkedVehicle.Vehicle.Model}");
        }

        ConsoleInput.Pause();
    }

    public void ListVehicleTypes()
    {
        bool foundVehicle = false;

        foreach (var group in garage.VehiclesByType)
        {
            foundVehicle = true;
            Console.WriteLine(
                $"{group.VehicleType}: {group.Vehicles.Length}");
        }

        if (!foundVehicle)
        {
            Console.WriteLine("The garage is empty.");
        }

        ConsoleInput.Pause();
    }

    public void FindVehicleByRegistration()
    {
        string registrationNumber =
            ConsoleInput.ReadRequiredString(
                "Registration number: ");

        var match = garage
            .FindVehicleByRegistration(registrationNumber);

        if (match is null)
        {
            Console.WriteLine("No vehicle was found.");
        }
        else
        {
            PrintVehicle(match.Value.SpotIndex + 1, match.Value.Vehicle);  
        }

        ConsoleInput.Pause();
    }

    public void RemoveVehicle()
    {
        string registrationNumber =
            ConsoleInput.ReadRequiredString(
                "Registration number: ");

        GarageOperationResult result =
            garage.Remove(registrationNumber);

        Console.WriteLine(result.Message);
        ConsoleInput.Pause();
    }

    private void Park(Vehicle vehicle)
    {
        GarageOperationResult result = garage.Park(vehicle);

        Console.WriteLine();
        Console.WriteLine(result.Message);
        ConsoleInput.Pause();
    }

    private void Park(Vehicle vehicle, int spot)
    {
        GarageOperationResult result = garage.Park(vehicle, spot);

        Console.WriteLine();
        Console.WriteLine(result.Message);
        ConsoleInput.Pause();
    }

    private static void PrintVehicle(
        int spotNumber,
        Vehicle vehicle)
    {
        Console.WriteLine($"Spot: {spotNumber}");
        Console.WriteLine($"Type: {vehicle.GetType().Name}");
        Console.WriteLine(
            $"Registration number: {vehicle.RegistrationNumber}");
        Console.WriteLine(
            $"Vehicle: {vehicle.Manufacturer} {vehicle.Model}");
        Console.WriteLine($"Year: {vehicle.ManufacturingYear}");
        Console.WriteLine($"Color: {vehicle.Color}");
        Console.WriteLine();
    }
}
