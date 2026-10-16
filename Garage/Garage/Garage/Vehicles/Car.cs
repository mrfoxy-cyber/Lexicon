using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Vehicle
{
    public class Car : Vehicle
    {
        public required Chassis Chassis { get; init; }

        public required Cabin Cabin { get; init; }
        public int NumberOfDoors { get; init; }
        public int NumberOfSeats { get; init; }
    }
}
