using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles
{
    public class Airplane : Vehicle<AirplaneSpecification>, IPassengerCarrier, IHasWheels, IHasSeats
    {
        public required string AirframeSerialNumber { get; init; }
        public int NumberOfWheels => Specification.NumberOfWheels;

        public int PassengerCapacity => Specification.Cabin.PassengerSeatCount;

        public int DriverSeats => Specification.Cabin.CrewSeatCount;

        public int NumberOfSeats =>
            PassengerCapacity +
            DriverSeats +
            (Specification.Cockpit?.PilotSeats ?? 0);
    }
}
