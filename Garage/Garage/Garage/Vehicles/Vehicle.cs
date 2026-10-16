using Garage.Engines;
using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Types;

namespace Garage.Vehicles
{

    public abstract class Vehicle
    {
        public required string RegistrationNumber { get; init; }
        public required string Manufacturer { get; init; }
        public required string Model { get; init; }

        public int ManufacturingYear { get; init; }
        public VehicleColor Color { get; set; }
        public double WeightKg { get; init; }
        public List<Engine> Engines { get; init; } = [];

        public int NumberOfWheels { get; init; }

        public double Length { get; init; }

        public double width { get; init; }
        public double height { get; init; }

    }
}
