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

    public void ListParkedVehicles()
    {
        var parkedVehicles = garage.ParkedVehicles.ToList();

        if (parkedVehicles.Count == 0)
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
                $"{group.VehicleType}: {group.Vehicles.Count}");
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

        var matches = garage
            .FindVehiclesByRegistration(registrationNumber)
            .ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("No vehicle was found.");
        }
        else
        {
            foreach (var match in matches)
            {
                PrintVehicle(match.SpotIndex + 1, match.Vehicle);
            }
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
