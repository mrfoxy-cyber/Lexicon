using Garage.Results;
using Garage.Vehicles;
using Garage.Search;
using Garage.Vehicles.Interfaces;

namespace Garage.Parking;

public class Garage<T> where T : Vehicle
{
    private readonly T?[] vehicles;
    public const int MaximumGarageCapacity = 1_000;

    public Garage(
        string name,
        int capacity,
        IEnumerable<T>? initialVehicles = null)
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

        List<T> startingVehicles =
            initialVehicles?.ToList() ?? [];

        if (startingVehicles.Count > capacity)
        {
            throw new ArgumentException(
                "There are more initial vehicles than parking spots.",
                nameof(initialVehicles));
        }

        bool hasDuplicateRegistrations = startingVehicles
            .GroupBy(
                vehicle => vehicle.RegistrationNumber,
                StringComparer.OrdinalIgnoreCase)
            .Any(group => group.Count() > 1);

        if (hasDuplicateRegistrations)
        {
            throw new ArgumentException(
                "Initial vehicles contain duplicate registration numbers.",
                nameof(initialVehicles));
        }

        GarageName = name;
        vehicles = new T?[capacity];

        for (int index = 0;
             index < startingVehicles.Count;
             index++)
        {
            vehicles[index] = startingVehicles[index];
        }
    }


    public string GarageName { get; }

    public int Capacity => vehicles.Length;

    public int OccupiedSpaces =>
        vehicles.Count(vehicle => vehicle is not null);

    public int AvailableSpaces => Capacity - OccupiedSpaces;

    public IEnumerable<T> Vehicles => vehicles.OfType<T>();

    public GarageOperationResult Park(T vehicle)
    {
        if (FindVehiclesByRegistration(
         vehicle.RegistrationNumber).Any())
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

    private static int NextOpenSpace(T?[] vehicles)
    {
        return Array.FindIndex(
            vehicles,
            vehicle => vehicle is null);
    }

    public IEnumerable<(int SpotIndex, T Vehicle)>
        FindVehiclesByRegistration(string registrationNumber)
    {
        for (int index = 0; index < vehicles.Length; index++)
        {
            if (vehicles[index] is T vehicle &&
                vehicle.RegistrationNumber.Equals(
                    registrationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                yield return (
                    SpotIndex: index,
                    Vehicle: vehicle
                );
            }
        }
    }

    public GarageOperationResult Remove(string registrationNumber)
    {
        var matches = FindVehiclesByRegistration(
            registrationNumber).ToList();

        if (matches.Count == 0)
        {
            return new GarageOperationResult(
                false,
                null,
                "We could not find your vehicle.");
        }

        if (matches.Count > 1)
        {
            return new GarageOperationResult(
                false,
                null,
                "More than one vehicle has this registration number. " +
                "Choose a parking spot instead.");
        }

        var match = matches[0];

        vehicles[match.SpotIndex] = null;

        return new GarageOperationResult(
            true,
            match.SpotIndex,
            $"Vehicle {match.Vehicle.RegistrationNumber} was removed " +
            $"from parking spot {match.SpotIndex + 1}.");
    }

    public IEnumerable<(int SpotNumber, T Vehicle)> ParkedVehicles
    {
        get
        {
            for (int index = 0; index < vehicles.Length; index++)
            {
                if (vehicles[index] is T vehicle)
                {
                    yield return (index + 1, vehicle);
                }
            }
        }
    }

    public IEnumerable<(string VehicleType, IReadOnlyList<T> Vehicles)> VehiclesByType
    {
        get
        {
            var groups = Vehicles
                .GroupBy(vehicle => vehicle.GetType().Name)
                .OrderBy(group => group.Key);

            foreach (var group in groups)
            {
                yield return (
                    VehicleType: group.Key,
                    Vehicles: group.ToList()
                );
            }
        }
    }

    public IEnumerable<T> Search(VehicleSearchCriteria criteria)
    {
        foreach (T vehicle in Vehicles)
        {
            if (criteria.RegistrationNumber is not null &&
                !vehicle.RegistrationNumber.Equals(
                    criteria.RegistrationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (criteria.Manufacturer is not null &&
                !vehicle.Manufacturer.Contains(
                    criteria.Manufacturer,
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (criteria.Model is not null &&
                !vehicle.Model.Contains(
                    criteria.Model,
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (criteria.ManufacturingYear is not null &&
                vehicle.ManufacturingYear != criteria.ManufacturingYear)
            {
                continue;
            }

            if (criteria.Color is not null &&
                vehicle.Color != criteria.Color)
            {
                continue;
            }

            if (criteria.VehicleType is not null &&
                !criteria.VehicleType.IsInstanceOfType(vehicle))
            {
                continue;
            }

            if (criteria.NumberOfWheels is not null)
            {
                if (vehicle is not IHasWheels vehicleWithWheels)
                {
                    continue;
                }

                if (vehicleWithWheels.NumberOfWheels !=
                    criteria.NumberOfWheels)
                {
                    continue;
                }
            }

            yield return vehicle;
        }
    }
}
