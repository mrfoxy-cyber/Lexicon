using Garage.ConsoleApp.Data;
using Garage.Vehicles;
using VehicleGarage = Garage.Parking.Garage<Garage.Vehicles.Vehicle>;

try
{
    List<Vehicle> startingVehicles = SeedData.LoadVehicles();

    VehicleGarage garage = new(
        name: "Lexicon Garage",
        capacity: 20,
        initialVehicles: startingVehicles);

    Console.WriteLine(
        $"{garage.GarageName} started with " +
        $"{garage.OccupiedSpaces} parked vehicles.");
}
catch (Exception exception) when (
    exception is IOException or
    System.Text.Json.JsonException or
    ArgumentException)
{
    Console.WriteLine(
        $"The garage could not be started: {exception.Message}");
}
