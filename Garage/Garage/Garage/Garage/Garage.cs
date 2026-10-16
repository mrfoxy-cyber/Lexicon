using System;
using System.Collections.Generic;
using System.Text;
using Vehicle;

namespace Garage
{
    internal class Garage<T> where T : Vehicle.Vehicle
    {
        private readonly List<T> vehicles = [];

        public void Park(T vehicle) => vehicles.Add(vehicle);

        public IReadOnlyCollection<T> Vehicles => vehicles;
    }
}
