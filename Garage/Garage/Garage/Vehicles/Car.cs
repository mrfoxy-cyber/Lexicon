using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Components;

namespace Garage.Vehicles
{
    public class Car : Vehicle, IPassengerCarrier
    {
        public required Chassis Chassis { get; init; }

        public required Cabin Cabin { get; init; }
        public int NumberOfDoors { get; init; }
        public int PassengerCapacity => Cabin.PassengerSeatCount;

        public int DriverSeats => Cabin.CrewSeatCount;
    }
}
