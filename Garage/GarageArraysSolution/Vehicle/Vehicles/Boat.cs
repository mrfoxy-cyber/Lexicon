using System;
using System.Collections.Generic;
using System.Text;

using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles
{
    public class Boat : Vehicle, IPassengerCarrier, IHasSeats
    {
        public required BoatSpecification Specification { get; init; }
        public override VehicleSpecification SpecificationDetails => Specification;
        public required string Hin { get; init; }

        public int PassengerCapacity => Specification.PassengerCapacity;
        public bool HasCabin => Specification.Cabin is not null;

        public int DriverSeats => Specification.DriverSeats;

        public int NumberOfSeats => PassengerCapacity + DriverSeats;
    }
}
