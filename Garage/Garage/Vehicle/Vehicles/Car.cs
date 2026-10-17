using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles
{
    public class Car : Vehicle<CarSpecification>, IPassengerCarrier, IHasWheels
    {
        public required string Vin { get; init; }
        public int PassengerCapacity => Specification.Cabin.PassengerSeatCount;
        public int NumberOfWheels => Specification.NumberOfWheels;

        public int DriverSeats => Specification.Cabin.CrewSeatCount;
    }
}
