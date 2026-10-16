using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Garage.Vehicles.Types;

namespace Garage.Vehicles.Components
{

    public class Chassis
    {
        public required string Vin { get; init; }
        public required string Manufacturer { get; init; }
        public required string Model { get; init; }

        public int ManufacturingYear { get; init; }
        public VehicleColor Color { get; set; }
        public BodyType BodyType { get; init; }

        public int NumberOfDoors { get; init; }
        public int NumberOfSeats { get; init; }
        public double WeightKg { get; init; }


    }
}
