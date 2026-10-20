using Garage.Vehicles;
using VehicleGarage = Garage.Parking.Garage;

namespace Garage.ConsoleApp.GarageUI;

public static class VehicleParkingStatus
{
    public static bool IsParked(
        Vehicle vehicle,
        VehicleGarage[] garages)
    {
        for (int index = 0; index < garages.Length; index++)
        {
            if (garages[index].FindVehicleByRegistration(
                vehicle.RegistrationNumber) is not null)
            {
                return true;
            }
        }

        return false;
    }
}
