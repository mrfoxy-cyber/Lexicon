using Garage.Vehicles;
using System.Net.Quic;

namespace Garage.Parking;

internal class Garage<T> where T : Vehicle
{
    private readonly T?[] vehicles;

    public required string GarageName { get; set; }

    public Garage(string name, int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero");
        
        vehicles = new T[capacity];
        GarageName = name;
    }

    public int Capacity => vehicles.Length;

    public int OccupiedSpaces => vehicles.Count(vehicle => vehicle is not null);

    public int availableSpaces => Capacity - OccupiedSpaces;

    public IEnumerable<T> Vehicles => vehicles.OfType<T>();

    public ParkResult Park(T vehicle)
    {
        if (FindAllByRegistration(vehicle.RegistrationNumber).Any())
        {
            return new ParkResult
            (
                false,
                null,
                "Error, there exist in the system car with such licence plate"
            );
        }
        else
        {
            if (this.vehicles is null)
            {
                return new ParkResult
                    (
                        false,
                        null,
                        "Where is your garage?"
                    );

            }

            int availableSpotIndex = NextOpenSpace(this.vehicles);

            if (availableSpotIndex==-1)
            {
                return new ParkResult
                    (
                        false,
                        null,
                        "I am sorry, Garage is full"
                    );

            } else
            {
                return new ParkResult
                (
                    true,
                    availableSpotIndex,
                    $"Car parked at spot: {availableSpotIndex}"
                );
            }

        }
    }

    public static int NextOpenSpace(T?[] vehicles)
    {
            return Array.FindIndex(
                                   vehicles,
                                   vehicle => vehicle is null);
    }

    public IEnumerable<T> FindAllByRegistration(string registrationNumber)
    {
        return Vehicles.Where(vehicle =>
            vehicle.RegistrationNumber.Equals(
                registrationNumber,
                StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<int> FindVehicleSpotsByReg(string registrationNumber)
    {
        return Enumerable
            .Range(0, vehicles.Length)
            .Where(index =>
                vehicles[index] is not null &&
                vehicles[index]!.RegistrationNumber.Equals(
                    registrationNumber,
                    StringComparison.OrdinalIgnoreCase));
    }

    public ParkResult Remove(string registrationNumber)
    {
        List<int> matchingSpots =
            FindVehicleSpotsByReg(registrationNumber).ToList();

        if (matchingSpots.Count == 0)
        {
            return new ParkResult(
                false,
                null,
                "We could not find your vehicle.");
        }

        if (matchingSpots.Count > 1)
        {
            return new ParkResult(
                false,
                null,
                "More than one vehicle has this registration number. " +
                "Choose a parking spot instead.");
        }

        int spotIndex = matchingSpots[0];
        T? vehicleToRemove = vehicles[spotIndex];

        if (vehicleToRemove is null)
        {
            return new ParkResult(
                false,
                null,
                "The parking spot is empty.");
        }

        string registration = vehicleToRemove.RegistrationNumber;

        vehicles[spotIndex] = null;

        return new ParkResult(
            true,
            spotIndex,
            $"Vehicle {registration} was successfully removed " +
            $"from parking spot {spotIndex + 1}.");
    }
}