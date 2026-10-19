using Garage.ConsoleApp.ConsoleFramework;
using Garage.Vehicles;
using VehicleGarage = Garage.Parking.Garage<Garage.Vehicles.Vehicle>;

namespace Garage.ConsoleApp.GarageUI;

public sealed class GarageApplication
{
    private readonly List<VehicleGarage> garages;

    public GarageApplication(
        IEnumerable<VehicleGarage> initialGarages)
    {
        garages = initialGarages.ToList();
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
        if (garages.Count == 0)
        {
            Console.WriteLine("There are no garages yet.");
            ConsoleInput.Pause();
            return;
        }

        List<MenuOption> garageOptions = garages
            .Select(garage => new MenuOption(
                $"{garage.GarageName} " +
                $"({garage.OccupiedSpaces}/{garage.Capacity})",
                () => OpenGarage(garage)))
            .ToList();

        ConsoleMenu garageMenu = new(
            title: "Choose garage",
            options: garageOptions);

        garageMenu.Show();
    }

    private void CreateGarage()
    {
        string name = ConsoleInput.ReadRequiredString(
            "Garage name: ");

        bool nameAlreadyExists = garages.Any(garage =>
            garage.GarageName.Equals(
                name,
                StringComparison.OrdinalIgnoreCase));

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

        garages.Add(garage);
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
