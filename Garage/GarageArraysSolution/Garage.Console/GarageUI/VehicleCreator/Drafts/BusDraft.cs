namespace Garage.ConsoleApp.GarageUI;

internal sealed class BusDraft : VehicleDraft
{
    public string? Vin { get; set; }
    public int? NumberOfDoors { get; set; }
    public int? NumberOfWheels { get; set; }
    public int? PassengerSeats { get; set; }
    public int? StandingPassengerCapacity { get; set; }
    public int? DriverSeats { get; set; }

    public override string[] GetMissingFields()
    {
        string[] missing = base.GetMissingFields();
        if (string.IsNullOrWhiteSpace(Vin)) AddMissingField(ref missing, "VIN");
        if (NumberOfDoors is null) AddMissingField(ref missing, "number of doors");
        if (NumberOfWheels is null) AddMissingField(ref missing, "number of wheels");
        if (PassengerSeats is null) AddMissingField(ref missing, "passenger seats");
        if (StandingPassengerCapacity is null) AddMissingField(ref missing, "standing passenger capacity");
        if (DriverSeats is null) AddMissingField(ref missing, "driver seats");
        return missing;
    }

    public override void Clear()
    {
        base.Clear();
        Vin = null;
        NumberOfDoors = null;
        NumberOfWheels = null;
        PassengerSeats = null;
        StandingPassengerCapacity = null;
        DriverSeats = null;
    }
}
