using Garage.Engines;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Types;
using Garage.Vehicles.Components;

namespace Garage.Vehicles
{

    public abstract class Vehicle : IManufacturer
    {
        public required string RegistrationNumber { get; init; }
        public required string Manufacturer { get; init; }
        public required string Model { get; init; }

        public int ManufacturingYear { get; init; }
        public VehicleColor Color { get; set; }
        public double WeightKg { get; init; }
        public required Dimensions Dimensions { get; init; }
        public List<Engine> Engines { get; init; } = [];

    }
}
