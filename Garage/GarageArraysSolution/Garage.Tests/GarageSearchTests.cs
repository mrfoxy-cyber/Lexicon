using Garage.Search;
using Garage.Tests.Fixtures;
using Garage.Vehicles;
using Garage.Vehicles.Types;
using VehicleGarage = Garage.Parking.Garage;

namespace Garage.Tests;

public class GarageSearchTests
{
    [Fact]
    public void Search_CombinesCriteriaWithAndLogic()
    {
        Vehicle matchingCar = VehicleFixtures.CreateCar(
            "MATCH1",
            VehicleColor.Black,
            manufacturer: "Volvo");

        Vehicle wrongColor = VehicleFixtures.CreateCar(
            "COLOR1",
            VehicleColor.Red,
            manufacturer: "Volvo");

        Vehicle wrongType = VehicleFixtures.CreateMotorcycle(
            "TYPE1",
            VehicleColor.Black);

        VehicleGarage garage = new(
            "Test garage",
            5,
            [matchingCar, wrongColor, wrongType]);

        VehicleSearchCriteria criteria = new()
        {
            VehicleTypes = [typeof(Car)],
            Manufacturer = "vol",
            Color = VehicleColor.Black,
            NumberOfWheels = 4
        };

        Vehicle[] matches = garage.Search(criteria);

        Assert.Same(matchingCar, Assert.Single(matches));
    }

    [Fact]
    public void Search_ReturnsEmptyCollectionWhenNothingMatches()
    {
        VehicleGarage garage = new(
            "Test garage",
            2,
            [VehicleFixtures.CreateCar()]);

        Vehicle[] matches = garage.Search(
            new VehicleSearchCriteria
            {
                Color = VehicleColor.Pink
            });

        Assert.Empty(matches);
    }

    [Fact]
    public void VehiclesByType_GroupsAndCountsVehicles()
    {
        VehicleGarage garage = new(
            "Test garage",
            4,
            [
                VehicleFixtures.CreateCar("CAR001"),
                VehicleFixtures.CreateCar("CAR002"),
                VehicleFixtures.CreateMotorcycle("MCY001")
            ]);

        var carGroup = garage.VehiclesByType.Single(
            group => group.VehicleType == nameof(Car));

        var motorcycleGroup = garage.VehiclesByType.Single(
            group => group.VehicleType == nameof(Motorcycle));

        Assert.Equal(2, carGroup.Vehicles.Count());
        Assert.Single(motorcycleGroup.Vehicles);
    }
}
