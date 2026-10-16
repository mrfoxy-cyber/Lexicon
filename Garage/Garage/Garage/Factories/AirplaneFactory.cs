using Garage.Engines;
using Garage.Vehicles;
using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Factories;

public static class AirplaneFactory
{
    public static Airplane CreateAirplane(
        string registrationNumber,
        string manufacturer,
        string model,
        int manufacturingYear,
        VehicleColor color,
        double weightKg,
        int numberOfWheels,
        double lengthMeters,
        double wingspanMeters,
        double heightMeters,
        string airframeSerialNumber,
        int passengerSeatCount,
        int crewSeatCount,
        int pilotSeatCount,
        bool hasDualControls,
        double maximumAltitudeMeters,
        IEnumerable<Engine>? engines = null)
    {
        Cockpit? cockpit = pilotSeatCount > 0
            ? new Cockpit
            {
                PilotSeats = pilotSeatCount,
                HasDualControls = hasDualControls
            }
            : null;

        return new Airplane
        {
            RegistrationNumber = registrationNumber,
            Manufacturer = manufacturer,
            Model = model,
            ManufacturingYear = manufacturingYear,
            Color = color,
            WeightKg = weightKg,
            NumberOfWheels = numberOfWheels,
            Length = lengthMeters,
            width = wingspanMeters,
            height = heightMeters,

            PassengerCapacity = passengerSeatCount,
            MaximumAltitudeMeters = maximumAltitudeMeters,

            Airframe = new Airframe
            {
                SerialNumber = airframeSerialNumber,
                LengthMeters = lengthMeters,
                WingspanMeters = wingspanMeters,
                Cockpit = cockpit
            },

            Cabin = new Cabin
            {
                PassengerSeatCount = passengerSeatCount,
                CrewSeatCount = crewSeatCount
            },

            Engines = engines?.ToList() ?? []
        };
    }
}
