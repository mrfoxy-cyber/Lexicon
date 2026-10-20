using Garage.Vehicles.Components;

namespace Garage.Vehicles.Specifications;

public class BusSpecification : VehicleSpecification
{
    public BusSpecification(
        int numberOfDoors,
        int numberOfWheels,
        int standingPassengerCapacity,
        Cabin cabin)
    {
        if (numberOfDoors < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(numberOfDoors),
                "A bus must have at least one door.");
        }

        if (numberOfWheels < 2)
        {
            throw new ArgumentOutOfRangeException(
                nameof(numberOfWheels),
                "A bus must have at least two wheels.");
        }

        if (cabin.PassengerSeatCount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cabin),
                "Passenger seat count cannot be negative.");
        }

        if (standingPassengerCapacity < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(standingPassengerCapacity),
                "Standing passenger capacity cannot be negative.");
        }

        if (cabin.CrewSeatCount < 1)
        {
            throw new ArgumentException(
                "A bus must have at least one driver seat.",
                nameof(cabin));
        }

        NumberOfDoors = numberOfDoors;
        NumberOfWheels = numberOfWheels;
        StandingPassengerCapacity = standingPassengerCapacity;
        Cabin = cabin;
    }

    public int NumberOfDoors { get; }
    public int NumberOfWheels { get; }
    public int StandingPassengerCapacity { get; }
    public Cabin Cabin { get; }
}
