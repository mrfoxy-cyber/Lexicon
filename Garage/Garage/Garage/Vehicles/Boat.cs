using System;
using System.Collections.Generic;
using System.Text;

using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Vehicles
{
    public class Boat : Vehicle, IPassengerCarrier
    {
        public required Hull Hull { get; init; }
        public Cabin? Cabin { get; init; }

        public BoatType BoatType { get; init; }
        public int PassengerCapacity { get; init; }
        public bool HasCabin => Cabin is not null;

        public int DriverSeats { get; init; }
    }
}
