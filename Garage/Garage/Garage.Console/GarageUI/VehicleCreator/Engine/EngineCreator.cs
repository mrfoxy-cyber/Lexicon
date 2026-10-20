using Garage.ConsoleApp.ConsoleFramework;
using Garage.Engines;

namespace Garage.ConsoleApp.GarageUI;

public sealed class EngineCreator
{
    private readonly EngineActions engineActions = new();
    private readonly EngineDraft draft = new();

    public void ShowCreateEngineMenu(Action<Engine> handleCreatedEngine)
    {
        EngineKind? kind =
            ConsoleInput.ReadEnumOrCancel<EngineKind>(
                "Choose engine type:");

        if (kind is null)
            return;

        draft.Clear();
        draft.Kind = kind;

        ConsoleMenu menu = new(
            title: $"Configure {kind}",
            options: CreateOptions(handleCreatedEngine));

        menu.Show();
    }

    private MenuOption[] CreateOptions(
        Action<Engine> handleCreatedEngine)
    {
        List<MenuOption> options =
        [
            new MenuOption(
                () => $"Serial number: {draft.SerialNumberText}",
                ChooseSerialNumber),

            new MenuOption(
                () => $"Model: {draft.ModelText}",
                ChooseModel)
        ];

        switch (draft.Kind)
        {
            case EngineKind.Inline:
            case EngineKind.VEngine:
            case EngineKind.Flat:
                AddFuelOption(options);
                AddHorsePowerOption(options);
                AddDisplacementOption(options);
                options.Add(new MenuOption(
                    () => $"Cylinders: {draft.CylinderCountText}",
                    ChooseCylinderCount));
                break;

            case EngineKind.Rotary:
                AddFuelOption(options);
                AddHorsePowerOption(options);
                AddDisplacementOption(options);
                options.Add(new MenuOption(
                    () => $"Rotors: {draft.RotorCountText}",
                    ChooseRotorCount));
                break;

            case EngineKind.ElectricMotor:
                options.Add(new MenuOption(
                    () => $"Power: {draft.PowerText} kW",
                    ChoosePower));
                options.Add(new MenuOption(
                    () => $"Voltage: {draft.VoltageText} V",
                    ChooseVoltage));
                break;

            case EngineKind.Steam:
                AddFuelOption(options);
                AddHorsePowerOption(options);
                options.Add(new MenuOption(
                    () => $"Boiler pressure: {draft.BoilerPressureText} bar",
                    ChooseBoilerPressure));
                break;

            case EngineKind.Turbojet:
                AddTurbineOptions(options);
                break;

            case EngineKind.Turbofan:
                AddTurbineOptions(options);
                options.Add(new MenuOption(
                    () => $"Bypass ratio: {draft.BypassRatioText}",
                    ChooseBypassRatio));
                break;

            case EngineKind.Turboprop:
            case EngineKind.Turboshaft:
                AddTurbineOptions(options);
                options.Add(new MenuOption(
                    () => $"Shaft horsepower: {draft.ShaftHorsePowerText}",
                    ChooseShaftHorsePower));
                break;
        }

        options.Add(new MenuOption(
            "Create engine",
            () => CreateEngine(handleCreatedEngine)));

        options.Add(new MenuOption(
            "Clear engine fields",
            ClearFields));

        return [.. options];
    }

    private void CreateEngine(Action<Engine> handleCreatedEngine)
    {
        EngineCreatorResult result =
            engineActions.CreateEngine(draft);

        Console.WriteLine(result.Message);

        if (result.Success)
        {
            foreach (Engine engine in result.Engines)
                handleCreatedEngine(engine);

            ClearFields();
        }

        ConsoleInput.Pause();
    }

    private void ClearFields()
    {
        EngineKind? kind = draft.Kind;
        draft.Clear();
        draft.Kind = kind;
    }

    private void AddFuelOption(List<MenuOption> options)
    {
        options.Add(new MenuOption(
            () => $"Fuel: {draft.FuelTypeText}",
            ChooseFuelType));
    }

    private void AddHorsePowerOption(List<MenuOption> options)
    {
        options.Add(new MenuOption(
            () => $"Horsepower: {draft.HorsePowerText}",
            ChooseHorsePower));
    }

    private void AddDisplacementOption(List<MenuOption> options)
    {
        options.Add(new MenuOption(
            () => $"Displacement: {draft.DisplacementText} cc",
            ChooseDisplacement));
    }

    private void AddTurbineOptions(List<MenuOption> options)
    {
        AddFuelOption(options);
        options.Add(new MenuOption(
            () => $"Thrust: {draft.ThrustText} kN",
            ChooseThrust));
    }

    private void ChooseSerialNumber()
    {
        draft.SerialNumber = ConsoleInput.ReadRequiredString(
            "Engine serial number: ");
    }

    private void ChooseModel()
    {
        draft.Model = ConsoleInput.ReadRequiredString(
            "Engine model: ");
    }

    private void ChooseFuelType()
    {
        draft.FuelType = ConsoleInput.ReadEnum<FuelType>(
            "Fuel type:");
    }

    private void ChooseHorsePower()
    {
        draft.HorsePower = ConsoleInput.ReadInt(
            "Horsepower: ", 1, 100_000);
    }

    private void ChooseDisplacement()
    {
        draft.DisplacementCc = ConsoleInput.ReadInt(
            "Displacement in cc: ", 1, 1_000_000);
    }

    private void ChooseCylinderCount()
    {
        draft.CylinderCount = ConsoleInput.ReadInt(
            "Number of cylinders: ", 1, 100);
    }

    private void ChooseRotorCount()
    {
        draft.RotorCount = ConsoleInput.ReadInt(
            "Number of rotors: ", 1, 10);
    }

    private void ChoosePower()
    {
        draft.PowerKw = ConsoleInput.ReadDouble(
            "Power in kW: ", 0.1, 100_000);
    }

    private void ChooseVoltage()
    {
        draft.Voltage = ConsoleInput.ReadDouble(
            "Voltage: ", 1, 100_000);
    }

    private void ChooseBoilerPressure()
    {
        draft.BoilerPressureBar = ConsoleInput.ReadDouble(
            "Boiler pressure in bar: ", 0.1, 10_000);
    }

    private void ChooseThrust()
    {
        draft.ThrustKn = ConsoleInput.ReadDouble(
            "Thrust in kN: ", 0.01, 1_000_000);
    }

    private void ChooseBypassRatio()
    {
        draft.BypassRatio = ConsoleInput.ReadDouble(
            "Bypass ratio: ", 0, 100);
    }

    private void ChooseShaftHorsePower()
    {
        draft.ShaftHorsePower = ConsoleInput.ReadInt(
            "Shaft horsepower: ", 1, 100_000);
    }
}
