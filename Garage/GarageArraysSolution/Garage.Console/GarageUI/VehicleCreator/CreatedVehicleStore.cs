using Garage.Vehicles;

namespace Garage.ConsoleApp.GarageUI;

public sealed class CreatedVehicleStore
{
    private Vehicle[] vehicles;

    public CreatedVehicleStore(Vehicle[]? initialVehicles = null)
    {
        vehicles = initialVehicles is null
            ? []
            : [.. initialVehicles];
    }

    public Vehicle[] Vehicles => [.. vehicles];

    public void Add(Vehicle vehicle)
    {
        vehicles = [.. vehicles, vehicle];
    }

    public bool Remove(Vehicle vehicle)
    {
        int index = Array.IndexOf(vehicles, vehicle);

        if (index < 0)
        {
            return false;
        }

        vehicles =
        [
            .. vehicles[..index],
            .. vehicles[(index + 1)..]
        ];

        return true;
    }
}
