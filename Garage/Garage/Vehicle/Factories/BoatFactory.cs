using Garage.Engines;
using Garage.Vehicles;
using Garage.Vehicles.Components;
using Garage.Vehicles.Specifications;
using Garage.Vehicles.Types;

namespace Garage.Factories;

public static class BoatFactory
{
    public static Boat CreateBoat(
        string registrationNumber,
        string manufacturer,
        string model,
        int manufacturingYear,
        VehicleColor color,
        double weightKg,
        double lengthMeters,
        double widthMeters,
        double heightMeters,
        string hin,
        double draftMeters,
        BoatType boatType,
        int passengerCapacity,
        int driverSeats,
        bool hasCabin,
        int cabinPassengerSeatCount,
        int cabinCrewSeatCount,
        Engine[]? engines = null)
    {
        Cabin? cabin = hasCabin
            ? new Cabin
            {
                PassengerSeatCount = cabinPassengerSeatCount,
                CrewSeatCount = cabinCrewSeatCount
            }
            : null;

        return new Boat
        {
            RegistrationNumber = registrationNumber,
            Manufacturer = manufacturer,
            Model = model,
            ManufacturingYear = manufacturingYear,
            Color = color,
            Hin = hin,
            Specification = new BoatSpecification
            {
                WeightKg = weightKg,
                Dimensions = new Dimensions(lengthMeters, widthMeters, heightMeters),
                BoatType = boatType,
                DraftMeters = draftMeters,
                PassengerCapacity = passengerCapacity,
                DriverSeats = driverSeats,
                Cabin = cabin,
                Engines = engines ?? []
            }
        };
    }
}
