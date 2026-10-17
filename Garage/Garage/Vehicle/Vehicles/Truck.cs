using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles;

public class Truck : Vehicle<TruckSpecification>, IHasWheels
{
    public required string Vin { get; init; }

    public int NumberOfWheels =>
        Specification.NumberOfWheels;
}
