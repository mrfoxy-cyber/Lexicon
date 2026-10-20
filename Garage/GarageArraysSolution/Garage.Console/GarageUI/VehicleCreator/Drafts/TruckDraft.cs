namespace Garage.ConsoleApp.GarageUI;

internal sealed class TruckDraft : VehicleDraft
{
    public string? Vin { get; set; }
    public int? NumberOfWheels { get; set; }
    public int? NumberOfAxles { get; set; }
    public double? MaximumLoadKg { get; set; }

    public override string[] GetMissingFields()
    {
        string[] missing = base.GetMissingFields();
        if (string.IsNullOrWhiteSpace(Vin)) AddMissingField(ref missing, "VIN");
        if (NumberOfWheels is null) AddMissingField(ref missing, "number of wheels");
        if (NumberOfAxles is null) AddMissingField(ref missing, "number of axles");
        if (MaximumLoadKg is null) AddMissingField(ref missing, "maximum load");
        return missing;
    }

    public override void Clear()
    {
        base.Clear();
        Vin = null;
        NumberOfWheels = null;
        NumberOfAxles = null;
        MaximumLoadKg = null;
    }
}
