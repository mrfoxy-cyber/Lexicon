using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Garage.Vehicle
{
    public enum VehicleColor
    {
        Black,
        White,
        Gray,
        Silver,
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
        Brown,
        Purple
    }

    public enum BodyType
    {
        Sedan,
        Hatchback,
        SUV,
        Coupe,
        Convertible,
        Van,
        Pickup,
        Motorcycle
    }
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
