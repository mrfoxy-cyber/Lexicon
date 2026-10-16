using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Vehicle
{

    public abstract class Vehicle
    {
        public required string RegistrationNumber { get; init; }
        public required string Manufacturer { get; init; }
        public required string Model { get; init; }

        public int ManufacturingYear { get; init; }
        public VehicleColor Color { get; set; }
        public double WeightKg { get; init; }
        public List<Engine.Engine> Engines { get; init; } = [];

        public int[] parkingSpaceRequired=new int[3];

    }
}
