using System;
using System.Collections.Generic;
using System.Text;
namespace Garage.Parking
{
    internal class Garage<T> where T : Garage.Vehicles.Vehicle
    {
        private readonly T?[] vehicles;

        public Garage(int capacity)
        {
            vehicles = new T?[capacity];
        }

        public ParkResult Park(T vehicle)
        {
            int openSpot = Array.FindIndex(vehicles, spot => spot is null);

            if (openSpot == -1)
            {
                return new ParkResult(false, null, "The garage is full");
            }
            else
            {
                vehicles[openSpot] = vehicle;

                return new ParkResult(true, openSpot, "Parked the car in sport " + (openSpot));
            }

        }


        public IEnumerable<T> Vehicles =>
            vehicles.Where(vehicle => vehicle is not null)!;
    }

}
