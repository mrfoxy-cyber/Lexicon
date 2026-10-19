using Garage.Results;
using Garage.Search;
using Garage.Vehicles;
using Garage.Vehicles.Interfaces;
using System.Xml.Linq;

namespace Garage.Parking;

public class Garage<T> where T : Vehicle
{
    private readonly T?[] vehicles;
    public const int MaximumGarageCapacity = 1_000;

    public string GarageName { get; }

    public int Capacity { get; private  init; }

    public int OccupiedSpaces =>
        vehicles.Count(vehicle => vehicle is not null);

    public int AvailableSpaces => Capacity - OccupiedSpaces;

    public Vehicle[] Vehicles => [.. vehicles.OfType<T>()];

    public Garage(
        string garageName,
        int capacity)
        : this(
            garageName: garageName,
            capacity: capacity,
            initialVehicles: [])
    {
    }

    public Garage(
        string garageName,
        T?[] vehicles)
        : this(
            garageName: garageName,
            capacity: vehicles.Length,
            initialVehicles:
                [.. vehicles.OfType<T>()])
    {
    }

    public Garage(
    string garageName,
    int capacity,
    T[]? initialVehicles = null)
    {
        if (string.IsNullOrWhiteSpace(garageName))
        {
            throw new ArgumentException(
                "Garage name must not be empty.",
                nameof(garageName));
        }

        if (capacity < 1 ||
            capacity > MaximumGarageCapacity)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity),
                $"Capacity must be between 1 and " +
                $"{MaximumGarageCapacity}.");
        }

        T[] startingVehicles =
            initialVehicles ?? [];

        if (startingVehicles.Length > capacity)
        {
            throw new ArgumentException(
                "Too many initial vehicles.",
                nameof(initialVehicles));
        }

        this.GarageName = garageName;
        this.Capacity = capacity;
        this.vehicles = new T?[capacity];

        Array.Copy(
            startingVehicles,
            this.vehicles,
            startingVehicles.Length);
    }



    public GarageOperationResult ParkAtSpot( T vehicle, int spotNumber)
    {
        if (spotNumber < 1 ||
            spotNumber > Capacity)
        {
            return new GarageOperationResult(
                false,
                null,
                $"Parking spot must be between 1 and {Capacity}.");
        }

        if (FindVehiclesByRegistration(
                vehicle.RegistrationNumber).Any())
        {
            return new GarageOperationResult(
                false,
                null,
                "A vehicle with this registration number already exists.");
        }

        int spotIndex = spotNumber - 1;

        if (vehicles[spotIndex] is not null)
        {
            return new GarageOperationResult(
                false,
                spotIndex,
                $"Parking spot {spotNumber} is already occupied.");
        }

        vehicles[spotIndex] = vehicle;

        return new GarageOperationResult(
            true,
            spotIndex,
            $"Vehicle parked at spot {spotNumber}.");
    }


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

    public IEnumerable<(string VehicleType, Vehicle[] Vehicles)> VehiclesByType
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
                    Vehicles: group.ToArray()
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
