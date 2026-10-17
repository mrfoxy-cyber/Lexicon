using Garage.Engines;
using Garage.Vehicles;
using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Factories;

public static class BusFactory
{
    public static Bus CreateBus(
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
        int numberOfDoors,
        int numberOfWheels,
        int passengerSeatCount,
        int standingPassengerCapacity,
        int driverSeatCount,
        IEnumerable<Engine>? engines = null)
    {
        return new Bus
        {
            RegistrationNumber = registrationNumber,
            Manufacturer = manufacturer,
            Model = model,
            ManufacturingYear = manufacturingYear,
            Color = color,
            WeightKg = weightKg,
            Dimensions = new Dimensions(lengthMeters, widthMeters, heightMeters),
            NumberOfDoors = numberOfDoors,
            NumberOfWheels = numberOfWheels,
            StandingPassengerCapacity = standingPassengerCapacity,
            Chassis = new Chassis
            {
                Vin = vin
            },
            Cabin = new Cabin
            {
                PassengerSeatCount = passengerSeatCount,
                CrewSeatCount = driverSeatCount
            },
            Engines = engines?.ToList() ?? []
        };
    }
}
