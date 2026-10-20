using Garage.Engines;
using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.GarageUI;

public abstract class VehicleDraft
{
    public string? RegistrationNumber { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public int? ManufacturingYear { get; set; }
    public VehicleColor? Color { get; set; }
    public double? WeightKg { get; set; }
    public double? LengthMeters { get; set; }
    public double? WidthMeters { get; set; }
    public double? HeightMeters { get; set; }

    private Engine[] engines = [];

    public Engine[] Engines => [.. engines];

    public string RegistrationText =>
        RegistrationNumber ?? "Not set";

    public string ManufacturerText =>
        Manufacturer ?? "Not set";

    public string ModelText =>
        Model ?? "Not set";

    public string YearText =>
        ManufacturingYear?.ToString() ?? "Not set";

    public string ColorText =>
        Color?.ToString() ?? "Not set";

    public string WeightText =>
        WeightKg is null ? "Not set" : $"{WeightKg} kg";

    public string DimensionsText =>
        LengthMeters is null ||
        WidthMeters is null ||
        HeightMeters is null
            ? "Not set"
            : $"{LengthMeters} x {WidthMeters} x {HeightMeters} m";

    public string EnginesText => engines.Length == 0
        ? "No engine"
        : engines.Length == 1
            ? "1 engine"
            : $"{engines.Length} engines";

    public void AddEngines(Engine[] newEngines)
    {
        engines = [.. engines, .. newEngines];
    }

    public bool RemoveEngine(Engine engine)
    {
        int index = Array.IndexOf(engines, engine);

        if (index < 0)
        {
            return false;
        }

        engines =
        [
            .. engines[..index],
            .. engines[(index + 1)..]
        ];

        return true;
    }

    public virtual string[] GetMissingFields()
    {
        string[] missing = [];

        if (string.IsNullOrWhiteSpace(RegistrationNumber))
            AddMissingField(ref missing, "registration number");

        if (string.IsNullOrWhiteSpace(Manufacturer))
            AddMissingField(ref missing, "manufacturer");

        if (string.IsNullOrWhiteSpace(Model))
            AddMissingField(ref missing, "model");

        if (ManufacturingYear is null)
            AddMissingField(ref missing, "manufacturing year");

        if (Color is null)
            AddMissingField(ref missing, "color");

        if (WeightKg is null)
            AddMissingField(ref missing, "weight");

        if (LengthMeters is null ||
            WidthMeters is null ||
            HeightMeters is null)
        {
            AddMissingField(ref missing, "dimensions");
        }

        return missing;
    }

    protected static void AddMissingField(
        ref string[] missing,
        string fieldName)
    {
        Array.Resize(ref missing, missing.Length + 1);
        missing[^1] = fieldName;
    }

    public virtual void Clear()
    {
        RegistrationNumber = null;
        Manufacturer = null;
        Model = null;
        ManufacturingYear = null;
        Color = null;
        WeightKg = null;
        LengthMeters = null;
        WidthMeters = null;
        HeightMeters = null;
        engines = [];
    }
}
