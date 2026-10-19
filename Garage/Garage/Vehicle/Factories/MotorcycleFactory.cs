using Garage.Engines;
using Garage.Vehicles;
using Garage.Vehicles.Components;
using Garage.Vehicles.Specifications;
using Garage.Vehicles.Types;

namespace Garage.Factories;

public static class MotorcycleFactory
{
    public static Motorcycle CreateMotorcycle(
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
        MotorcycleType motorcycleType,
        bool hasSidecar,
        int numberOfWheels,
        int passengerCapacity,
        int driverSeats,
        Engine[]? engines = null)
    {
        return new Motorcycle
        {
            RegistrationNumber = registrationNumber,
            Manufacturer = manufacturer,
            Model = model,
            ManufacturingYear = manufacturingYear,
            Color = color,
            Vin = vin,
            Specification = new MotorcycleSpecification
            {
                WeightKg = weightKg,
                Dimensions = new Dimensions(lengthMeters, widthMeters, heightMeters),
                MotorcycleType = motorcycleType,
                HasSidecar = hasSidecar,
                NumberOfWheels = numberOfWheels,
                PassengerCapacity = passengerCapacity,
                DriverSeats = driverSeats,
                Engines = engines ?? []
            }
        };
    }
}
