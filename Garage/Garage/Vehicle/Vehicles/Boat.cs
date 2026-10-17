using System;
using System.Collections.Generic;
using System.Text;

using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles
{
    public class Boat : Vehicle<BoatSpecification>, IPassengerCarrier
    {
        public required string Hin { get; init; }

        public int PassengerCapacity => Specification.PassengerCapacity;
        public bool HasCabin => Specification.Cabin is not null;

        public int DriverSeats => Specification.DriverSeats;
    }
}
