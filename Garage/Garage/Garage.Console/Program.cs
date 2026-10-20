using Garage.ConsoleApp.Data;
using Garage.ConsoleApp.GarageUI;
using Garage.Vehicles;
using VehicleGarage =
    Garage.Parking.Garage<Garage.Vehicles.Vehicle>;

try
{
    Vehicle[] parkedVehicles =
        SeedData.LoadVehicles("vehicles.json");

    Vehicle[] createdVehicles =
        SeedData.LoadVehicles("created-vehicles.json");

    VehicleGarage startingGarage = new(
       garageName: "Lexicon Garage",
       capacity: 20,
        initialVehicles: parkedVehicles);

    GarageApplication application = new(
        initialGarages: [startingGarage],
        initialCreatedVehicles: createdVehicles);

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
