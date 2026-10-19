using Garage.ConsoleApp.Data;
using Garage.ConsoleApp.GarageUI;
using Garage.Vehicles;
using VehicleGarage =
    Garage.Parking.Garage<Garage.Vehicles.Vehicle>;

try
{
    List<Vehicle> startingVehicles =
        SeedData.LoadVehicles();

    VehicleGarage startingGarage = new(
        name: "Lexicon Garage",
        capacity: 20,
        initialVehicles: startingVehicles);

    GarageApplication application = new(
        initialGarages: [startingGarage]);

    application.Run();

    Console.WriteLine("Goodbye!");
}
catch (Exception exception) when (
    exception is IOException or
    System.Text.Json.JsonException or
    ArgumentException)
{
    Console.WriteLine(
        $"The garage could not be started: {exception.Message}");
}
