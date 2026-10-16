using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Garage.Vehicles.Components
{
    public class Airframe
    {
        public required string SerialNumber { get; init; }
        public double WingspanMeters { get; init; }

        public Cockpit? Cockpit { get; init; }
    }
}
