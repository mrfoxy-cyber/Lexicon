using Garage.Vehicles;

namespace Garage.ConsoleApp.GarageUI;

public sealed record VehicleCreatorResult(
    bool Success,
    bool Cancelled,
    Vehicle? Vehicle)
{
    public string Message
    {
        get
        {
            if (Cancelled)
            {
                return "Vehicle creation was cancelled.";
            }

            if (!Success || Vehicle is null)
            {
                return "The vehicle could not be created.";
            }

            return
                $"Created {Vehicle.GetType().Name}: " +
                $"{Vehicle.Manufacturer} {Vehicle.Model} " +
                $"({Vehicle.RegistrationNumber}).";
        }
    }

    public static VehicleCreatorResult Completed(Vehicle vehicle)
    {
        return new VehicleCreatorResult(
            Success: true,
            Cancelled: false,
            Vehicle: vehicle);
    }

    public static VehicleCreatorResult Cancel()
    {
        return new VehicleCreatorResult(
            Success: false,
            Cancelled: true,
            Vehicle: null);
    }
}
