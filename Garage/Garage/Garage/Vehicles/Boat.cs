using System;
using System.Collections.Generic;
using System.Text;

using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Vehicles
{
    internal class Boat: Garage.Vehicles.Vehicle, IPassengerCarrier
    {
        public required Hull Hull { get; init; }

        public BoatType BoatType { get; init; }
        public int PassengerCapacity { get; init; }
        public bool HasCabin { get; init; }

        public int DriverSeats { get; init; }
    }
}
