using Garage.Arrays.Results;
using Garage.Arrays.Search;
using Garage.Vehicles;
using Garage.Vehicles.Interfaces;

namespace Garage.Arrays.Parking;

public class Garage
{
    private readonly Vehicle?[] vehicles;

    public const int MaximumGarageCapacity = 100_000;

    public string GarageName { get; }
    public int Capacity { get; }

    public int OccupiedSpaces
    {
        get
        {
            int count = 0;

            for (int index = 0; index < vehicles.Length; index++)
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

    public Vehicle[] Vehicles
    {
        get
        {
            Vehicle[] parkedVehicles = new Vehicle[OccupiedSpaces];
            int resultIndex = 0;

            for (int index = 0; index < vehicles.Length; index++)
            {
                if (vehicles[index] is Vehicle vehicle)
                {
                    parkedVehicles[resultIndex] = vehicle;
                    resultIndex++;
                }
            }

            return parkedVehicles;
        }
    }

    public (int SpotNumber, Vehicle Vehicle)[] ParkedVehicles
    {
        get
        {
            (int SpotNumber, Vehicle Vehicle)[] parkedVehicles =
                new (int SpotNumber, Vehicle Vehicle)[OccupiedSpaces];

            int resultIndex = 0;

            for (int spotIndex = 0; spotIndex < vehicles.Length; spotIndex++)
            {
                if (vehicles[spotIndex] is Vehicle vehicle)
                {
                    parkedVehicles[resultIndex] = (
                        SpotNumber: spotIndex + 1,
                        Vehicle: vehicle);

                    resultIndex++;
                }
            }

            return parkedVehicles;
        }
    }

    public (string VehicleType, Vehicle[] Vehicles)[] VehiclesByType
    {
        get
        {
            Vehicle[] parkedVehicles = Vehicles;
            (string VehicleType, Vehicle[] Vehicles)[] groups = [];

            for (int vehicleIndex = 0;
                 vehicleIndex < parkedVehicles.Length;
                 vehicleIndex++)
            {
                Vehicle vehicle = parkedVehicles[vehicleIndex];
                string vehicleType = vehicle.GetType().Name;
                int groupIndex = FindGroupIndex(groups, vehicleType);

                if (groupIndex == -1)
                {
                    Array.Resize(ref groups, groups.Length + 1);
                    groups[^1] = (vehicleType, [vehicle]);
                    continue;
                }

                Vehicle[] vehiclesInGroup = groups[groupIndex].Vehicles;
                Array.Resize(ref vehiclesInGroup, vehiclesInGroup.Length + 1);
                vehiclesInGroup[^1] = vehicle;
                groups[groupIndex] = (vehicleType, vehiclesInGroup);
            }

            return groups;
        }
    }

    public Garage(string garageName, int capacity)
        : this(garageName, capacity, [])
    {
    }

    public Garage(string garageName, Vehicle[] vehicles)
        : this(garageName, vehicles.Length, vehicles)
    {
    }

    public Garage(
        string garageName,
        int capacity,
        Vehicle[]? initialVehicles = null)
    {
        if (string.IsNullOrWhiteSpace(garageName))
        {
            throw new ArgumentException(
                "Garage name must not be empty.",
                nameof(garageName));
        }

        if (capacity < 1 || capacity > MaximumGarageCapacity)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity),
                $"Capacity must be between 1 and {MaximumGarageCapacity}.");
        }

        Vehicle[] startingVehicles = initialVehicles ?? [];

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

        GarageName = garageName;
        Capacity = capacity;
        vehicles = new Vehicle?[capacity];

        Array.Copy(startingVehicles, vehicles, startingVehicles.Length);
    }

    public GarageOperationResult Park(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        int availableSpotIndex = NextOpenSpace();

        if (availableSpotIndex == -1)
        {
            return new GarageOperationResult(
                false,
                null,
                "The garage is full.");
        }

        return Park(vehicle, availableSpotIndex + 1);
    }

    public GarageOperationResult Park(Vehicle vehicle, int spot)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (spot < 1 || spot > Capacity)
        {
            return new GarageOperationResult(
                false,
                null,
                $"Parking spot must be between 1 and {Capacity}.");
        }

        int spotIndex = spot - 1;

        if (vehicles[spotIndex] is not null)
        {
            return new GarageOperationResult(
                false,
                null,
                $"Parking spot {spot} is occupied.");
        }

        if (RegistrationExists(vehicle.RegistrationNumber))
        {
            return new GarageOperationResult(
                false,
                null,
                $"A vehicle with registration number " +
                $"{vehicle.RegistrationNumber} is already parked.");
        }

        vehicles[spotIndex] = vehicle;

        return new GarageOperationResult(
            true,
            spotIndex,
            $"Vehicle {vehicle.RegistrationNumber} parked at spot {spot}.");
    }

    public (int SpotIndex, Vehicle Vehicle)? FindVehicleByRegistration(
        string registrationNumber)
    {
        for (int index = 0; index < vehicles.Length; index++)
        {
            if (vehicles[index] is Vehicle vehicle &&
                vehicle.RegistrationNumber.Equals(
                    registrationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                return (index, vehicle);
            }
        }

        return null;
    }

    public GarageOperationResult Remove(string registrationNumber)
    {
        var match = FindVehicleByRegistration(registrationNumber);

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
            return new GarageOperationResult(
                false,
                null,
                $"Parking spot must be between 1 and {Capacity}.");
        }

        Vehicle? vehicle = vehicles[spotNumber - 1];

        if (vehicle is null)
        {
            return new GarageOperationResult(
                false,
                null,
                $"Parking spot {spotNumber} is already empty.");
        }

        return Remove(vehicle.RegistrationNumber);
    }

    public Vehicle[] Search(VehicleSearchCriteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        int matchCount = 0;

        for (int index = 0; index < vehicles.Length; index++)
        {
            if (vehicles[index] is Vehicle vehicle &&
                MatchesCriteria(vehicle, criteria))
            {
                matchCount++;
            }
        }

        Vehicle[] matches = new Vehicle[matchCount];
        int matchIndex = 0;

        for (int index = 0; index < vehicles.Length; index++)
        {
            if (vehicles[index] is Vehicle vehicle &&
                MatchesCriteria(vehicle, criteria))
            {
                matches[matchIndex] = vehicle;
                matchIndex++;
            }
        }

        return matches;
    }

    private static int FindGroupIndex(
        (string VehicleType, Vehicle[] Vehicles)[] groups,
        string vehicleType)
    {
        for (int index = 0; index < groups.Length; index++)
        {
            if (groups[index].VehicleType == vehicleType)
            {
                return index;
            }
        }

        return -1;
    }

    private static bool HasDuplicateRegistrations(Vehicle[] vehicles)
    {
        for (int firstIndex = 0;
             firstIndex < vehicles.Length;
             firstIndex++)
        {
            for (int secondIndex = firstIndex + 1;
                 secondIndex < vehicles.Length;
                 secondIndex++)
            {
                if (vehicles[firstIndex].RegistrationNumber.Equals(
                    vehicles[secondIndex].RegistrationNumber,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool RegistrationExists(string registrationNumber)
    {
        return FindVehicleByRegistration(registrationNumber) is not null;
    }

    private int NextOpenSpace()
    {
        for (int index = 0; index < vehicles.Length; index++)
        {
            if (vehicles[index] is null)
            {
                return index;
            }
        }

        return -1;
    }

    private static bool MatchesCriteria(
        Vehicle vehicle,
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
            vehicle.ManufacturingYear != criteria.ManufacturingYear)
        {
            return false;
        }

        if (criteria.Color is not null && vehicle.Color != criteria.Color)
        {
            return false;
        }

        if (!MatchesVehicleType(vehicle, criteria.VehicleTypes))
        {
            return false;
        }

        if (criteria.NumberOfWheels is not null &&
            (vehicle is not IHasWheels vehicleWithWheels ||
             vehicleWithWheels.NumberOfWheels != criteria.NumberOfWheels))
        {
            return false;
        }

        var engines = vehicle.SpecificationDetails.Engines;

        if (criteria.NumberOfEngines is not null &&
            engines.Length != criteria.NumberOfEngines)
        {
            return false;
        }

        if (criteria.FuelType is not null)
        {
            bool fuelTypeMatches = false;

            for (int index = 0; index < engines.Length; index++)
            {
                if (engines[index].FuelType == criteria.FuelType)
                {
                    fuelTypeMatches = true;
                    break;
                }
            }

            if (!fuelTypeMatches)
            {
                return false;
            }
        }

        if (criteria.CylinderVolumeCc is not null)
        {
            bool cylinderVolumeMatches = false;

            for (int index = 0; index < engines.Length; index++)
            {
                double? cylinderVolume = engines[index].CylinderVolumeCc;

                if (cylinderVolume is not null &&
                    Math.Abs(
                        cylinderVolume.Value -
                        criteria.CylinderVolumeCc.Value) < 0.01)
                {
                    cylinderVolumeMatches = true;
                    break;
                }
            }

            if (!cylinderVolumeMatches)
            {
                return false;
            }
        }

        if (criteria.NumberOfSeats is not null &&
            (vehicle is not IHasSeats vehicleWithSeats ||
             vehicleWithSeats.NumberOfSeats != criteria.NumberOfSeats))
        {
            return false;
        }

        if (criteria.LengthMeters is not null &&
            Math.Abs(
                vehicle.SpecificationDetails.Dimensions.LengthMeters -
                criteria.LengthMeters.Value) >= 0.01)
        {
            return false;
        }

        return true;
    }

    private static bool MatchesVehicleType(
        Vehicle vehicle,
        Type[] vehicleTypes)
    {
        if (vehicleTypes.Length == 0)
        {
            return true;
        }

        for (int index = 0; index < vehicleTypes.Length; index++)
        {
            if (vehicleTypes[index].IsInstanceOfType(vehicle))
            {
                return true;
            }
        }

        return false;
    }
}
