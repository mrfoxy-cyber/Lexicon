using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Vehicles
{
    public class Car : Vehicle, IPassengerCarrier, IHasWheels
    {
        public required Chassis Chassis { get; init; }

        public required Cabin Cabin { get; init; }
        public BodyType BodyType { get; init; }
        public int NumberOfDoors { get; init; }
        public int NumberOfWheels { get; init; }
        public int PassengerCapacity => Cabin.PassengerSeatCount;

        public int DriverSeats => Cabin.CrewSeatCount;
    }
}
