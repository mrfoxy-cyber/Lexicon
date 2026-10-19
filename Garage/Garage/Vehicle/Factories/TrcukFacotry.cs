using Garage.Engines;
using Garage.Vehicles;
using Garage.Vehicles.Components;
using Garage.Vehicles.Specifications;
using Garage.Vehicles.Types;

namespace Garage.Factories;

public static class TruckFactory
{
    public static Truck CreateTruck(
        string registrationNumber,
        string manufacturer,
        string model,
        int manufacturingYear,
        VehicleColor color,
        double weightKg,
        double lengthMeters,
        double widthMeters,
        double heightMeters,
        string vin,
        int numberOfWheels,
        int numberOfAxles,
        double maximumLoadKg,
        Engine[]? engines = null)
    {
        return new Truck
        {
            RegistrationNumber = registrationNumber,
            Manufacturer = manufacturer,
            Model = model,
            ManufacturingYear = manufacturingYear,
            Color = color,
            Vin = vin,

            Specification = new TruckSpecification
            {
                WeightKg = weightKg,

                Dimensions = new Dimensions(
                    lengthMeters,
                    widthMeters,
                    heightMeters),

                NumberOfWheels = numberOfWheels,
                NumberOfAxles = numberOfAxles,
                MaximumLoadKg = maximumLoadKg,
                Engines = engines ?? []
            }
        };
    }
}