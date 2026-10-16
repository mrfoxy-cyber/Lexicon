using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Components;

namespace Garage.Vehicles
{
    public class Bus : Vehicle, IPassengerCarrier, IHasWheels
    {
        public required Chassis Chassis { get; init; }

        public required Cabin Cabin { get; init; }
        public int NumberOfDoors { get; init; }
        public int NumberOfWheels { get; init; }
        public int StandingPassengerCapacity { get; init; }

        public int PassengerCapacity =>
            Cabin.PassengerSeatCount + StandingPassengerCapacity;

        public int DriverSeats => Cabin.CrewSeatCount;
    }
}
