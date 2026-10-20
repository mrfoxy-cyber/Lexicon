using Garage.Factories;
using Garage.Vehicles;
using Garage.Vehicles.Types;

namespace Garage.Tests.Fixtures;

internal static class VehicleFixtures
{
    public static Car CreateCar(
        string registrationNumber = "ABC123",
        VehicleColor color = VehicleColor.Black,
        string manufacturer = "Volvo",
        int manufacturingYear = 2020)
    {
        return CarFactory.CreateCar(
            registrationNumber,
            manufacturer,
            "V70",
            manufacturingYear,
            color,
            1_700,
            4.8,
            1.8,
            1.5,
            $"VIN-{registrationNumber}",
            BodyType.Sedan,
            5,
            5);
    }

    public static Motorcycle CreateMotorcycle(
        string registrationNumber = "MCY123",
        VehicleColor color = VehicleColor.Red,
        int numberOfWheels = 2)
    {
        return MotorcycleFactory.CreateMotorcycle(
            registrationNumber,
            "Honda",
            "Gold Wing",
            2022,
            color,
            390,
            2.6,
            0.9,
            1.4,
            $"VIN-{registrationNumber}",
            MotorcycleType.Touring,
            false,
            numberOfWheels,
            1,
            1);
    }

    public static Bus CreateBus(
        int passengerSeats = 30,
        int standingCapacity = 20,
        int driverSeats = 1)
    {
        return BusFactory.CreateBus(
            "BUS123",
            "Scania",
            "Citywide",
            2023,
            VehicleColor.Blue,
            12_000,
            12,
            2.55,
            3.2,
            "VIN-BUS123",
            3,
            6,
            passengerSeats,
            standingCapacity,
            driverSeats);
    }

    public static Airplane CreateAirplane(
        int pilotSeats = 2,
        bool hasDualControls = true)
    {
        return AirplaneFactory.CreateAirplane(
            "AIR123",
            "Cessna",
            "172",
            2021,
            VehicleColor.White,
            767,
            3,
            8.3,
            11,
            2.7,
            "AIRFRAME-123",
            2,
            0,
            pilotSeats,
            hasDualControls,
            4_200);
    }

    public static Boat CreateBoat(
        int passengerCapacity = 8,
        int cabinPassengerSeats = 4)
    {
        return BoatFactory.CreateBoat(
            "SEA123",
            "Nimbus",
            "T11",
            2022,
            VehicleColor.White,
            5_600,
            12.4,
            3.5,
            3.2,
            "HIN-SEA123",
            0.9,
            BoatType.Motorboat,
            passengerCapacity,
            1,
            true,
            cabinPassengerSeats,
            1);
    }

    public static Truck CreateTruck(
        int numberOfWheels = 6,
        int numberOfAxles = 3,
        double maximumLoadKg = 25_000)
    {
        return TruckFactory.CreateTruck(
            "TRK123",
            "Volvo",
            "FH16",
            2023,
            VehicleColor.Red,
            9_000,
            7.1,
            2.5,
            3.8,
            "VIN-TRK123",
            numberOfWheels,
            numberOfAxles,
            maximumLoadKg);
    }
}
