namespace Garage.ConsoleApp.GarageUI;

public sealed class AirplaneDraft : VehicleDraft
{
    public double? WingspanMeters
    {
        get => WidthMeters;
        set => WidthMeters = value;
    }

    public string? AirframeSerialNumber { get; set; }
    public int? NumberOfWheels { get; set; }
    public int? PassengerSeats { get; set; }
    public int? CrewSeats { get; set; }
    public int? PilotSeats { get; set; }
    public bool? HasDualControls { get; set; }
    public double? MaximumAltitudeMeters { get; set; }

    public override string[] GetMissingFields()
    {
        string[] missing = base.GetMissingFields();
        if (string.IsNullOrWhiteSpace(AirframeSerialNumber)) AddMissingField(ref missing, "airframe serial number");
        if (NumberOfWheels is null) AddMissingField(ref missing, "number of wheels");
        if (PassengerSeats is null) AddMissingField(ref missing, "passenger seats");
        if (CrewSeats is null) AddMissingField(ref missing, "crew seats");
        if (PilotSeats is null) AddMissingField(ref missing, "pilot seats");
        if (HasDualControls is null) AddMissingField(ref missing, "dual controls choice");
        if (MaximumAltitudeMeters is null) AddMissingField(ref missing, "maximum altitude");
        return missing;
    }

    public override void Clear()
    {
        base.Clear();
        AirframeSerialNumber = null;
        NumberOfWheels = null;
        PassengerSeats = null;
        CrewSeats = null;
        PilotSeats = null;
        HasDualControls = null;
        MaximumAltitudeMeters = null;
    }
}
