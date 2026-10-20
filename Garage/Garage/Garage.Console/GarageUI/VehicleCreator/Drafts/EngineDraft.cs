using Garage.Engines;

namespace Garage.ConsoleApp.GarageUI;

public sealed class EngineDraft
{
    public EngineKind? Kind { get; set; }
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public FuelType? FuelType { get; set; }
    public int? HorsePower { get; set; }
    public int? DisplacementCc { get; set; }
    public int? CylinderCount { get; set; }
    public int? RotorCount { get; set; }
    public double? PowerKw { get; set; }
    public double? Voltage { get; set; }
    public double? BoilerPressureBar { get; set; }
    public double? ThrustKn { get; set; }
    public double? BypassRatio { get; set; }
    public int? ShaftHorsePower { get; set; }

    public string SerialNumberText => SerialNumber ?? "Not set";
    public string ModelText => Model ?? "Not set";
    public string FuelTypeText => FuelType?.ToString() ?? "Not set";
    public string HorsePowerText => HorsePower?.ToString() ?? "Not set";
    public string DisplacementText => DisplacementCc?.ToString() ?? "Not set";
    public string CylinderCountText => CylinderCount?.ToString() ?? "Not set";
    public string RotorCountText => RotorCount?.ToString() ?? "Not set";
    public string PowerText => PowerKw?.ToString() ?? "Not set";
    public string VoltageText => Voltage?.ToString() ?? "Not set";
    public string BoilerPressureText => BoilerPressureBar?.ToString() ?? "Not set";
    public string ThrustText => ThrustKn?.ToString() ?? "Not set";
    public string BypassRatioText => BypassRatio?.ToString() ?? "Not set";
    public string ShaftHorsePowerText => ShaftHorsePower?.ToString() ?? "Not set";

    public string[] GetMissingFields()
    {
        List<string> missing = [];

        if (Kind is null)
            missing.Add("engine type");

        if (string.IsNullOrWhiteSpace(SerialNumber))
            missing.Add("serial number");

        if (string.IsNullOrWhiteSpace(Model))
            missing.Add("model");

        switch (Kind)
        {
            case EngineKind.Inline:
            case EngineKind.VEngine:
            case EngineKind.Flat:
                RequireFuel(missing);
                RequireHorsePower(missing);
                RequireDisplacement(missing);
                if (CylinderCount is null)
                    missing.Add("cylinder count");
                break;

            case EngineKind.Rotary:
                RequireFuel(missing);
                RequireHorsePower(missing);
                RequireDisplacement(missing);
                if (RotorCount is null)
                    missing.Add("rotor count");
                break;

            case EngineKind.ElectricMotor:
                if (PowerKw is null)
                    missing.Add("power");
                if (Voltage is null)
                    missing.Add("voltage");
                break;

            case EngineKind.Steam:
                RequireFuel(missing);
                RequireHorsePower(missing);
                if (BoilerPressureBar is null)
                    missing.Add("boiler pressure");
                break;

            case EngineKind.Turbojet:
                RequireTurbineFields(missing);
                break;

            case EngineKind.Turbofan:
                RequireTurbineFields(missing);
                if (BypassRatio is null)
                    missing.Add("bypass ratio");
                break;

            case EngineKind.Turboprop:
            case EngineKind.Turboshaft:
                RequireTurbineFields(missing);
                if (ShaftHorsePower is null)
                    missing.Add("shaft horsepower");
                break;
        }

        return [.. missing];
    }

    public void Clear()
    {
        Kind = null;
        SerialNumber = null;
        Model = null;
        FuelType = null;
        HorsePower = null;
        DisplacementCc = null;
        CylinderCount = null;
        RotorCount = null;
        PowerKw = null;
        Voltage = null;
        BoilerPressureBar = null;
        ThrustKn = null;
        BypassRatio = null;
        ShaftHorsePower = null;
    }

    private void RequireFuel(List<string> missing)
    {
        if (FuelType is null)
            missing.Add("fuel type");
    }

    private void RequireHorsePower(List<string> missing)
    {
        if (HorsePower is null)
            missing.Add("horsepower");
    }

    private void RequireDisplacement(List<string> missing)
    {
        if (DisplacementCc is null)
            missing.Add("displacement");
    }

    private void RequireTurbineFields(List<string> missing)
    {
        RequireFuel(missing);
        if (ThrustKn is null)
            missing.Add("thrust");
    }
}
