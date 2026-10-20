using Garage.Tests.Fixtures;
using Garage.Vehicles;
using VehicleGarage = Garage.Parking.Garage<Garage.Vehicles.Vehicle>;

namespace Garage.Tests;

public class GarageParkingTests
{
    [Fact]
    public void Park_AddsVehicleAndUpdatesSpaceCounts()
    {
        VehicleGarage garage = new("Test garage", 2);
        Vehicle car = VehicleFixtures.CreateCar();

        var result = garage.Park(car);

        Assert.True(result.Success);
        Assert.Contains(car, garage.Vehicles);
        Assert.Equal(1, garage.OccupiedSpaces);
        Assert.Equal(1, garage.AvailableSpaces);
    }

    [Fact]
    public void Park_FailsWhenGarageIsFull()
    {
        VehicleGarage garage = new("Test garage", 1);
        garage.Park(VehicleFixtures.CreateCar("ONE123"));

        var result = garage.Park(
            VehicleFixtures.CreateCar("TWO123"));

        Assert.False(result.Success);
        Assert.Single(garage.Vehicles);
    }

    [Fact]
    public void Park_RejectsDuplicateRegistrationIgnoringCase()
    {
        VehicleGarage garage = new("Test garage", 2);
        garage.Park(VehicleFixtures.CreateCar("ABC123"));

        var result = garage.Park(
            VehicleFixtures.CreateCar("abc123"));

        Assert.False(result.Success);
        Assert.Single(garage.Vehicles);
    }

    [Fact]
    public void ParkAtSpot_UsesRequestedSpot()
    {
        VehicleGarage garage = new("Test garage", 3);
        Vehicle car = VehicleFixtures.CreateCar();

        var result = garage.Park(car, spot: 2);
        var parkedVehicle = Assert.Single(garage.ParkedVehicles);

        Assert.True(result.Success);
        Assert.Equal(2, parkedVehicle.SpotNumber);
        Assert.Same(car, parkedVehicle.Vehicle);
    }

    [Fact]
    public void ParkAtSpot_FailsWhenSpotIsOccupied()
    {
        VehicleGarage garage = new("Test garage", 2);
        garage.Park(VehicleFixtures.CreateCar("ONE123"), spot: 1);

        var result = garage.Park(
            VehicleFixtures.CreateCar("TWO123"),
            spot: 1);

        Assert.False(result.Success);
        Assert.Single(garage.Vehicles);
    }
}
