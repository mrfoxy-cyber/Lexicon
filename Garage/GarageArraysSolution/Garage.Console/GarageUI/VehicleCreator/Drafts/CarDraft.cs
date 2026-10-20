using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.GarageUI;

internal sealed class CarDraft : VehicleDraft
{
    public string? Vin { get; set; }
    public BodyType? BodyType { get; set; }
    public int? NumberOfDoors { get; set; }
    public int? NumberOfSeats { get; set; }

    public string VinText => Vin ?? "Not set";

    public string BodyTypeText =>
        BodyType?.ToString() ?? "Not set";

    public string DoorsText =>
        NumberOfDoors?.ToString() ?? "Not set";

    public string SeatsText =>
        NumberOfSeats?.ToString() ?? "Not set";

    public override string[] GetMissingFields()
    {
        string[] missing = base.GetMissingFields();

        if (string.IsNullOrWhiteSpace(Vin))
            AddMissingField(ref missing, "VIN");

        if (BodyType is null)
            AddMissingField(ref missing, "body type");

        if (NumberOfDoors is null)
            AddMissingField(ref missing, "number of doors");

        if (NumberOfSeats is null)
            AddMissingField(ref missing, "number of seats");

        return missing;
    }

    public override void Clear()
    {
        base.Clear();
        Vin = null;
        BodyType = null;
        NumberOfDoors = null;
        NumberOfSeats = null;
    }
}
