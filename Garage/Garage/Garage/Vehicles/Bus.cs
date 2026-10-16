using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Components;

namespace Garage.Vehicles
{
    internal class Bus : Vehicle, IPassengerCarrier
    {
        public required Chassis Chassis { get; init; }

        public required Cabin Cabin { get; init; }
        public int NumberOfDoors { get; init; }
        public int SeatedPassengerCapacity { get; init; }
        public int StandingPassengerCapacity { get; init; }

        public int PassengerCapacity =>
            SeatedPassengerCapacity + StandingPassengerCapacity;

        public int DriverSeats => Cabin.CrewSeatCount;
    }
}
