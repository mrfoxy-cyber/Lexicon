using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles
{
    public class Airplane : Vehicle<AirplaneSpecification>, IPassengerCarrier, IHasWheels
    {
        public required string AirframeSerialNumber { get; init; }
        public int NumberOfWheels => Specification.NumberOfWheels;

        public int PassengerCapacity => Specification.Cabin.PassengerSeatCount;

        public int DriverSeats => Specification.Cabin.CrewSeatCount;
    }
}
