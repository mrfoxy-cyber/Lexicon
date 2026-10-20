using Garage.Vehicles;

namespace Garage.ConsoleApp.GarageUI;

internal sealed record VehicleCreatorResult(
    bool Success,
    bool Cancelled,
    Vehicle? Vehicle,
    string Message)
{
    public static VehicleCreatorResult Completed(Vehicle vehicle)
    {
        return new VehicleCreatorResult(
            Success: true,
            Cancelled: false,
            Vehicle: vehicle,
            Message:
                $"Created {vehicle.GetType().Name}: " +
                $"{vehicle.Manufacturer} {vehicle.Model} " +
                $"({vehicle.RegistrationNumber}).");
    }

    public static VehicleCreatorResult Failed(string message)
    {
        return new VehicleCreatorResult(
            Success: false,
            Cancelled: false,
            Vehicle: null,
            Message: message);
    }

    public static VehicleCreatorResult Cancel()
    {
        return new VehicleCreatorResult(
            Success: false,
            Cancelled: true,
            Vehicle: null,
            Message: "Vehicle creation was cancelled.");
    }
}
