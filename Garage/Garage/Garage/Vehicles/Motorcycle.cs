using System;
using System.Collections.Generic;
using System.Text;
using Garage.Vehicles.Components;
using Garage.Vehicles.Types;

namespace Garage.Vehicles;

public class Motorcycle : Vehicle, IPassengerCarrier, IHasWheels
{
    public required Chassis Chassis { get; init; }
    public MotorcycleType MotorcycleType { get; init; }
    public bool HasSidecar { get; init; }
    public int NumberOfWheels { get; init; }

    public int PassengerCapacity { get; init; }

    public int DriverSeats { get; init; }
}
