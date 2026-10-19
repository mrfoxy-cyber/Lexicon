using Garage.ConsoleApp.ConsoleFramework;
using Garage.Parking;
using Garage.Vehicles;
using VehicleGarage = Garage.Parking.Garage<Garage.Vehicles.Vehicle>;

namespace Garage.ConsoleApp.GarageUI;

public sealed class GarageApplication
{
    private VehicleGarage[] garages;

    public VehicleGarage[] Garages =>
        [.. garages];

    public GarageApplication(
        VehicleGarage[] initialGarages)
    {
        garages = [.. initialGarages];
    }

    private void AddGarage(
        VehicleGarage garage)
    {
        garages = [.. garages, garage];
    }

    public void Run()
    {
        ConsoleMenu startMenu = new(
            title: "Garage system",
            options:
            [
                new MenuOption(
                    "Choose garage",
                    ChooseGarage),

                new MenuOption(
                    "Create garage",
                    CreateGarage)
            ],
            backOptionText: "Exit");

        startMenu.Show();
    }

    private void ChooseGarage()
    {
        if (garages.Length == 0)
        {
            Console.WriteLine("There are no garages yet.");
            ConsoleInput.Pause();
            return;
        }

        MenuOption[] garageOptions = new MenuOption[garages.Length];

        for(int i=0; i < garages.Length; i++)
        {
            VehicleGarage selectedGarage = garages[i];

            var menuOption= new MenuOption(
                $"{garages[i].GarageName} ({garages[i].OccupiedSpaces}/{garages[i].Capacity})",
                () => OpenGarage(selectedGarage)
                );

            garageOptions[i] = menuOption;
        }



        ConsoleMenu garageMenu = new(
            title: "Choose garage",
            options: garageOptions);

        garageMenu.Show();
    }

    private void CreateGarage()
    {
        string name = ConsoleInput.ReadRequiredString(
            "Garage name: ");

        bool nameAlreadyExists = false;

        for (int i = 0; i < garages.Length; i++)
        {
            if (garages[i].GarageName.Equals(
                name,
                StringComparison.OrdinalIgnoreCase))
            {
                nameAlreadyExists = true;
                break;
            }
        }

        if (nameAlreadyExists)
        {
            Console.WriteLine(
                "A garage with that name already exists.");
            ConsoleInput.Pause();
            return;
        }

        int capacity = ConsoleInput.ReadInt(
            "Number of parking spots: ",
            minimum: 1,
            maximum: int.MaxValue);

        VehicleGarage garage = new(
            garageName: name,
            capacity: capacity);

        AddGarage(garage);
        OpenGarage(garage);
    }

    private static void OpenGarage(VehicleGarage garage)
    {
        GarageActions actions = new(garage);

        ConsoleMenu vehicleMenu = new(
            title: garage.GarageName,
            options:
            [
                new MenuOption(
                    "Park vehicle",
                    actions.ParkVehicle),

                new MenuOption(
                    "Park vehicle to specific spot",
                    actions.ParkVehicleAtSpot),

                new MenuOption(
                    "List all parked vehicles",
                    actions.ListParkedVehicles),

                new MenuOption(
                    "List vehicle types",
                    actions.ListVehicleTypes),

                new MenuOption(
                    "Find vehicle by registration number",
                    actions.FindVehicleByRegistration),

                new MenuOption(
                    "Remove vehicle",
                    actions.RemoveVehicle)
            ],
            backOptionText: "Choose another garage");

        vehicleMenu.Show();
    }
}
