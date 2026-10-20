using Garage.ConsoleApp.ConsoleFramework;
using Garage.Vehicles;

namespace Garage.ConsoleApp.GarageUI;

public sealed class VehicleCreator
{
    private readonly VehicleActions vehicleActions = new();

    public VehicleCreatorResult CreateVehicle()
    {
        Vehicle? vehicle = ChooseAndCreateVehicle();

        return vehicle is null
            ? VehicleCreatorResult.Cancel()
            : VehicleCreatorResult.Completed(vehicle);
    }

    private Vehicle? ChooseAndCreateVehicle()
    {
        VehicleKind? selectedVehicle =
            ConsoleInput.ReadEnumOrCancel<VehicleKind>(
                "Choose vehicle type:");

        if (selectedVehicle is null)
        {
            return null;
        }

        return selectedVehicle.Value switch
        {
            VehicleKind.Car =>
                vehicleActions.CreateCar(),

            VehicleKind.Motorcycle =>
                vehicleActions.CreateMotorcycle(),

            VehicleKind.Airplane =>
                vehicleActions.CreateAirplane(),

            VehicleKind.Bus =>
                vehicleActions.CreateBus(),

            VehicleKind.Boat =>
                vehicleActions.CreateBoat(),

            VehicleKind.Truck =>
                vehicleActions.CreateTruck(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(selectedVehicle),
                selectedVehicle,
                "Unknown vehicle type.")
        };
    }
}
