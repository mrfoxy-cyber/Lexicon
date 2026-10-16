using Garage.Engines;
using Garage.Vehicles;
using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Factories;

public static class CarFactory
{
    public static Car CreateCar(
        string registrationNumber,
        string manufacturer,
        string model,
        int manufacturingYear,
        VehicleColor color,
        double weightKg,
        double length,
        double width,
        double height,
        string vin,
        BodyType bodyType,
        int numberOfDoors,
        int numberOfSeats,
        IEnumerable<Engine>? engines = null)
    {
        return new Car
        {
            RegistrationNumber = registrationNumber,
            Manufacturer = manufacturer,
            Model = model,
            ManufacturingYear = manufacturingYear,
            Color = color,
            WeightKg = weightKg,
            NumberOfWheels = 4,
            Length = length,
            width = width,
            height = height,

            NumberOfDoors = numberOfDoors,
            NumberOfSeats = numberOfSeats,

            Chassis = new Chassis
            {
                Vin = vin,
                Manufacturer = manufacturer,
                Model = model,
                ManufacturingYear = manufacturingYear,
                Color = color,
                BodyType = bodyType,
                NumberOfDoors = numberOfDoors,
                NumberOfSeats = numberOfSeats,
                WeightKg = weightKg
            },

            Cabin = new Cabin
            {
                PassengerSeatCount = numberOfSeats - 1,
                CrewSeatCount = 1
            },

            Engines = engines?.ToList() ?? []
        };
    }
}
