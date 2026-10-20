using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.GarageUI;

internal sealed class BoatDraft : VehicleDraft
{
    public string? Hin { get; set; }
    public double? DraftMeters { get; set; }
    public BoatType? BoatType { get; set; }
    public int? PassengerCapacity { get; set; }
    public int? DriverSeats { get; set; }
    public bool? HasCabin { get; set; }
    public int? CabinPassengerSeats { get; set; }
    public int? CabinCrewSeats { get; set; }

    public override string[] GetMissingFields()
    {
        string[] missing = base.GetMissingFields();
        if (string.IsNullOrWhiteSpace(Hin)) AddMissingField(ref missing, "HIN");
        if (DraftMeters is null) AddMissingField(ref missing, "draft");
        if (BoatType is null) AddMissingField(ref missing, "boat type");
        if (PassengerCapacity is null) AddMissingField(ref missing, "passenger capacity");
        if (DriverSeats is null) AddMissingField(ref missing, "driver seats");
        if (HasCabin is null) AddMissingField(ref missing, "cabin choice");

        if (HasCabin == true)
        {
            if (CabinPassengerSeats is null) AddMissingField(ref missing, "cabin passenger seats");
            if (CabinCrewSeats is null) AddMissingField(ref missing, "cabin crew seats");
        }

        return missing;
    }

    public override void Clear()
    {
        base.Clear();
        Hin = null;
        DraftMeters = null;
        BoatType = null;
        PassengerCapacity = null;
        DriverSeats = null;
        HasCabin = null;
        CabinPassengerSeats = null;
        CabinCrewSeats = null;
    }
}
