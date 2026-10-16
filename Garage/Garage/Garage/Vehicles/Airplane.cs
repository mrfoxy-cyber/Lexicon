using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Components;

namespace Garage.Vehicles
{
    public class Airplane : Vehicle, IPassengerCarrier
    {
        public required Airframe Airframe { get; init; }

        public required Cabin Cabin { get; init; }
        public double MaximumAltitudeMeters { get; init; }

        public int PassengerCapacity => Cabin.PassengerSeatCount;

        public int DriverSeats => Cabin.CrewSeatCount;
    }
}
