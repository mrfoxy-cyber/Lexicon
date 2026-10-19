using Garage.Results;
using Garage.Search;
using Garage.Vehicles;
using Garage.Vehicles.Interfaces;

namespace Garage.Parking;

public class Garage<T> where T : Vehicle
{
    private readonly T?[] vehicles;
    public const int MaximumGarageCapacity = 1_000;

    public string GarageName { get; }

    public int Capacity { get; private  init; }

    public int OccupiedSpaces
    {
        get
        {
            int count = 0;

            for (int index = 0;
                 index < vehicles.Length;
                 index++)
            {
                if (vehicles[index] is not null)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public int AvailableSpaces => Capacity - OccupiedSpaces;

    public T[] Vehicles
    {
        get
        {
            T[] parkedVehicles =
                new T[OccupiedSpaces];

            int parkedVehicleIndex = 0;

            for (int index = 0;
                 index < vehicles.Length;
                 index++)
            {
                if (vehicles[index] is T vehicle)
                {
                    parkedVehicles[parkedVehicleIndex] =
                        vehicle;

                    parkedVehicleIndex++;
                }
            }

            return parkedVehicles;
        }
    }

    private static bool HasDuplicateRegistrations(
    T[] vehicles)
    {
        for (int firstIndex = 0;
             firstIndex < vehicles.Length;
             firstIndex++)
        {
            for (int secondIndex = firstIndex + 1;
                 secondIndex < vehicles.Length;
                 secondIndex++)
            {
                bool registrationsMatch =
                    vehicles[firstIndex]
                        .RegistrationNumber
                        .Equals(
                            vehicles[secondIndex]
                                .RegistrationNumber,
                            StringComparison.OrdinalIgnoreCase);

                if (registrationsMatch)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool RegistrationExists(
    string registrationNumber)
    {
        for (int index = 0;
             index < vehicles.Length;
             index++)
        {
            T? parkedVehicle = vehicles[index];

            if (parkedVehicle is not null &&
                parkedVehicle.RegistrationNumber.Equals(
                    registrationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public (int SpotIndex, T Vehicle)? FindVehicleByRegistration( string registrationNumber)
    {
        for (int index = 0;
             index < vehicles.Length;
             index++)
        {
            if (vehicles[index] is T vehicle &&
                vehicle.RegistrationNumber.Equals(
                    registrationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                return (
                    SpotIndex: index,
                    Vehicle: vehicle);
            }
        }

        return null;
    }

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
        T[] vehicles)
        : this(
            garageName: garageName,
            capacity: vehicles.Length,
            initialVehicles: vehicles)
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

        if (HasDuplicateRegistrations(startingVehicles))
        {
            throw new ArgumentException(
                "Initial vehicles contain duplicate registration numbers.",
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

    public GarageOperationResult Park(T vehicle)
    {
        int availableSpotIndex = NextOpenSpace(vehicles);

        if (availableSpotIndex == -1)
        {
            return new GarageOperationResult(
                false,
                null,
                "The garage is full.");
        }

        return Park(vehicle, availableSpotIndex+1);
    }

    public GarageOperationResult Park(T vehicle, int spot)
    {
        if (spot > Capacity|| spot < 1)
            return new GarageOperationResult
                (
                    false,
                    null,
                    $"I am sorry, but there doesnt exists {spot}"
                );

        if (vehicles[spot-1] is not null)
            return new GarageOperationResult
                    (
                        false,
                        null,
                        $"I am sorry, The spot {spot} is occupied"
                    );

        if (RegistrationExists(vehicle.RegistrationNumber))
        {
            return new GarageOperationResult(
                false,
                null,
                $"A vehicle with registration number " +
                $"{vehicle.RegistrationNumber} is already parked.");
        }

        vehicles[spot - 1] = vehicle;
        return new GarageOperationResult(
            true,
            spot-1,
            $"Vehcile {vehicle.RegistrationNumber} parked at sport {spot}"
            );
    }

    private static int NextOpenSpace(T?[] vehicles)
    {
        return Array.FindIndex(
            vehicles,
            vehicle => vehicle is null);
    }

    public GarageOperationResult Remove(string registrationNumber)
    {
        var match = FindVehicleByRegistration(
            registrationNumber);

        if (match is null)
        {
            return new GarageOperationResult(
                false,
                null,
                "We could not find your vehicle.");
        }

        vehicles[match.Value.SpotIndex] = null;

        return new GarageOperationResult(
            true,
            match.Value.SpotIndex,
            $"Vehicle {match.Value.Vehicle.RegistrationNumber} was removed " +
            $"from parking spot {match.Value.SpotIndex + 1}.");
    }

    public GarageOperationResult RemoveAtSpot(int spotNumber)
    {
        if (spotNumber < 1 || spotNumber > Capacity)
        {
            return new GarageOperationResult
            (
                false,
                null,
                $"Parking spot must be between 1 and {Capacity}."
            );
        }

        int spotIndex = spotNumber - 1;

        T? vehicle = vehicles[spotIndex];

        if(vehicle is null)
        {
            return new GarageOperationResult(
                false,
                null,
                 $"Parking spot {spotNumber} is already empty."
                );
        }

        return Remove(vehicle.RegistrationNumber);
    }

    public T[] Search(
    VehicleSearchCriteria criteria)
    {
        int matchCount = 0;

        for (int index = 0;
             index < vehicles.Length;
             index++)
        {
            if (vehicles[index] is T vehicle &&
                MatchesCriteria(vehicle, criteria))
            {
                matchCount++;
            }
        }

        T[] matches = new T[matchCount];

        int matchIndex = 0;

        for (int index = 0;
             index < vehicles.Length;
             index++)
        {
            if (vehicles[index] is T vehicle &&
                MatchesCriteria(vehicle, criteria))
            {
                matches[matchIndex] = vehicle;
                matchIndex++;
            }
        }

        return matches;
    }

    private static bool MatchesCriteria(
    T vehicle,
    VehicleSearchCriteria criteria)
    {
        if (criteria.RegistrationNumber is not null &&
            !vehicle.RegistrationNumber.Equals(
                criteria.RegistrationNumber,
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (criteria.Manufacturer is not null &&
            !vehicle.Manufacturer.Contains(
                criteria.Manufacturer,
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (criteria.Model is not null &&
            !vehicle.Model.Contains(
                criteria.Model,
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (criteria.ManufacturingYear is not null &&
            vehicle.ManufacturingYear !=
            criteria.ManufacturingYear)
        {
            return false;
        }

        if (criteria.Color is not null &&
            vehicle.Color != criteria.Color)
        {
            return false;
        }

        if (criteria.VehicleType is not null &&
            !criteria.VehicleType.IsInstanceOfType(vehicle))
        {
            return false;
        }

        if (criteria.NumberOfWheels is not null)
        {
            if (vehicle is not IHasWheels vehicleWithWheels)
            {
                return false;
            }

            if (vehicleWithWheels.NumberOfWheels !=
                criteria.NumberOfWheels)
            {
                return false;
            }
        }

        return true;
    }
}
