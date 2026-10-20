using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Types;
using Garage.Vehicles.Interfaces;
using Garage.Vehicles.Specifications;

namespace Garage.Vehicles
{

    public abstract class Vehicle : IManufacturer
    {
        public required string RegistrationNumber { get; init; }
        public required string Manufacturer { get; init; }
        public required string Model { get; init; }

        public required int ManufacturingYear { get; init; }
        public required VehicleColor Color { get; set; }

        public abstract VehicleSpecification SpecificationDetails { get; }
    }

}
