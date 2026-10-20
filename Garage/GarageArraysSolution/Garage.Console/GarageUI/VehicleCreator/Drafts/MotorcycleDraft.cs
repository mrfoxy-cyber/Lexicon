using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.GarageUI;

internal sealed class MotorcycleDraft : VehicleDraft
{
    public string? Vin { get; set; }
    public MotorcycleType? MotorcycleType { get; set; }
    public bool? HasSidecar { get; set; }
    public int? NumberOfWheels { get; set; }
    public int? PassengerCapacity { get; set; }
    public int? DriverSeats { get; set; }

    public override string[] GetMissingFields()
    {
        string[] missing = base.GetMissingFields();
        if (string.IsNullOrWhiteSpace(Vin)) AddMissingField(ref missing, "VIN");
        if (MotorcycleType is null) AddMissingField(ref missing, "motorcycle type");
        if (HasSidecar is null) AddMissingField(ref missing, "sidecar choice");
        if (NumberOfWheels is null) AddMissingField(ref missing, "number of wheels");
        if (PassengerCapacity is null) AddMissingField(ref missing, "passenger capacity");
        if (DriverSeats is null) AddMissingField(ref missing, "driver seats");
        return missing;
    }

    public override void Clear()
    {
        base.Clear();
        Vin = null;
        MotorcycleType = null;
        HasSidecar = null;
        NumberOfWheels = null;
        PassengerCapacity = null;
        DriverSeats = null;
    }
}
