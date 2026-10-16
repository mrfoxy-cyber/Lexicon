using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Vehicles.Components
{
    public class Hull
    {
        public required string IdentificationNumber { get; init; }
        public double DraftMeters { get; init; }
    }
}
