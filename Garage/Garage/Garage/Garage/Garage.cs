using System;
using System.Collections.Generic;
using System.Text;
namespace Garage
{
    internal class Garage<T> where T : Garage.Vehicle.Vehicle
    {
        private readonly T[] vehicles;

        public void Park(T vehicle) => vehicles.Add(vehicle);

        public IReadOnlyCollection<T> Vehicles => vehicles;
    }
}
