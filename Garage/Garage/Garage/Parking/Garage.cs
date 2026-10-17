using Garage.Results;
using Garage.Vehicles;

namespace Garage.Parking;

public class Garage<T> where T : Vehicle
{
    private readonly T?[] vehicles;

    public Garage(string name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Garage name must not be empty.",
                nameof(name));
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity),
                "Capacity must be greater than zero.");
        }

        GarageName = name;
        vehicles = new T?[capacity];
    }

    public string GarageName { get; }

    public int Capacity => vehicles.Length;

    public int OccupiedSpaces =>
        vehicles.Count(vehicle => vehicle is not null);

    public int AvailableSpaces => Capacity - OccupiedSpaces;

    public IEnumerable<T> Vehicles => vehicles.OfType<T>();

    public GarageOperationResult Park(T vehicle)
    {
        if (FindAllByRegistration(vehicle.RegistrationNumber).Any())
        {
            return new GarageOperationResult(
                false,
                null,
                "A vehicle with this licence plate already exists.");
        }

        int availableSpotIndex = NextOpenSpace(vehicles);

        if (availableSpotIndex == -1)
        {
            return new GarageOperationResult(
                false,
                null,
                "The garage is full.");
        }

        vehicles[availableSpotIndex] = vehicle;

        return new GarageOperationResult(
            true,
            availableSpotIndex,
            $"Vehicle parked at spot {availableSpotIndex + 1}.");
    }

    public static int NextOpenSpace(T?[] vehicles)
    {
        return Array.FindIndex(
            vehicles,
            vehicle => vehicle is null);
    }

    public IEnumerable<T> FindAllByRegistration(
        string registrationNumber)
    {
        return Vehicles.Where(vehicle =>
            vehicle.RegistrationNumber.Equals(
                registrationNumber,
                StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<int> FindVehicleSpotsByReg(
        string registrationNumber)
    {
        return Enumerable
            .Range(0, vehicles.Length)
            .Where(index =>
                vehicles[index] is not null &&
                vehicles[index]!.RegistrationNumber.Equals(
                    registrationNumber,
                    StringComparison.OrdinalIgnoreCase));
    }

    public GarageOperationResult Remove(string registrationNumber)
    {
        List<int> matchingSpots =
            FindVehicleSpotsByReg(registrationNumber).ToList();

        if (matchingSpots.Count == 0)
        {
            return new GarageOperationResult(
                false,
                null,
                "We could not find your vehicle.");
        }

        if (matchingSpots.Count > 1)
        {
            return new GarageOperationResult(
                false,
                null,
                "More than one vehicle has this registration number. " +
                "Choose a parking spot instead.");
        }

        int spotIndex = matchingSpots[0];
        T? vehicleToRemove = vehicles[spotIndex];

        if (vehicleToRemove is null)
        {
            return new GarageOperationResult(
                false,
                null,
                "The parking spot is empty.");
        }

        string registration = vehicleToRemove.RegistrationNumber;
        vehicles[spotIndex] = null;

        return new GarageOperationResult(
            true,
            spotIndex,
            $"Vehicle {registration} was successfully removed " +
            $"from parking spot {spotIndex + 1}.");
    }
}
