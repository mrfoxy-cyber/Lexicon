using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Vehicle
{
    public class Airplane : Vehicle
    {
        public required Airframe Airframe { get; init; }

        public required Cabin Cabin { get; init; }
        public int PassengerCapacity { get; init; }
        public double MaximumAltitudeMeters { get; init; }
    }
}
