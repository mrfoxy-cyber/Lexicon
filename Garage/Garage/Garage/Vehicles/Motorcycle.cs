using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles;

public class Motorcycle : Vehicle<MotorcycleSpecification>, IPassengerCarrier, IHasWheels
{
    public required string Vin { get; init; }
    public int NumberOfWheels => Specification.NumberOfWheels;

    public int PassengerCapacity => Specification.PassengerCapacity;

    public int DriverSeats => Specification.DriverSeats;
}
