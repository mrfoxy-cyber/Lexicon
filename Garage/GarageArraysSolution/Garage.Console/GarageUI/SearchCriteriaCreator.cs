using Garage.ConsoleApp.ConsoleFramework;
using Garage.Engines;
using Garage.Engines.Specifications;
using Garage.Search;
using Garage.Vehicles;
using Garage.Vehicles.Specifications;
using Garage.Vehicles.Types;

namespace Garage.ConsoleApp.GarageUI;

public sealed class SearchCriteriaCreator
{
    private readonly Type[] availableVehicleTypes =
    [
        typeof(Car),
        typeof(Motorcycle),
        typeof(Airplane),
        typeof(Bus),
        typeof(Boat),
        typeof(Truck)
    ];

    private readonly bool[] selectedVehicleTypes = new bool[6];
    private VehicleColor? color;
    private int? numberOfWheels;
    private int? numberOfEngines;
    private double? cylinderVolumeCc;
    private FuelType? fuelType;
    private int? numberOfSeats;
    private double? lengthMeters;
    private string? registrationNumber;
    private string? manufacturer;
    private string? model;
    private int? manufacturingYear;

    public string VehicleTypesText
    {
        get
        {
            string text = "";

            for (int index = 0;
                 index < availableVehicleTypes.Length;
                 index++)
            {
                if (!selectedVehicleTypes[index])
                {
                    continue;
                }

                if (text.Length > 0)
                {
                    text += ", ";
                }

                text += availableVehicleTypes[index].Name;
            }

            return text.Length == 0 ? "Any" : text;
        }
    }
    public string ColorText => color?.ToString() ?? "Any";
    public string NumberOfWheelsText =>
        numberOfWheels?.ToString() ?? "Any";
    public string NumberOfEnginesText =>
        numberOfEngines?.ToString() ?? "Any";
    public string CylinderVolumeText =>
        cylinderVolumeCc?.ToString() ?? "Any";
    public string FuelTypeText => fuelType?.ToString() ?? "Any";
    public string NumberOfSeatsText =>
        numberOfSeats?.ToString() ?? "Any";
    public string LengthText => lengthMeters?.ToString() ?? "Any";
    public string RegistrationNumberText =>
        registrationNumber ?? "Any";
    public string ManufacturerText => manufacturer ?? "Any";
    public string ModelText => model ?? "Any";
    public string ManufacturingYearText =>
        manufacturingYear?.ToString() ?? "Any";

    public VehicleSearchCriteria Create()
    {
        return new VehicleSearchCriteria
        {
            VehicleTypes = CreateSelectedVehicleTypes(),
            Color = color,
            NumberOfWheels = numberOfWheels,
            NumberOfEngines = numberOfEngines,
            CylinderVolumeCc = cylinderVolumeCc,
            FuelType = fuelType,
            NumberOfSeats = numberOfSeats,
            LengthMeters = lengthMeters,
            RegistrationNumber = registrationNumber,
            Manufacturer = manufacturer,
            Model = model,
            ManufacturingYear = manufacturingYear
        };
    }

    public void ChooseVehicleTypes()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Choose vehicle types ===");
            Console.WriteLine();

            for (int index = 0;
                 index < availableVehicleTypes.Length;
                 index++)
            {
                string marker = selectedVehicleTypes[index]
                    ? "X"
                    : " ";

                Console.WriteLine(
                    $"{index + 1}. [{marker}] " +
                    availableVehicleTypes[index].Name);
            }

            Console.WriteLine("0. Done");
            Console.WriteLine();

            int choice = ConsoleInput.ReadInt(
                "Toggle type: ",
                minimum: 0,
                maximum: availableVehicleTypes.Length);

            if (choice == 0)
            {
                return;
            }

            int selectedIndex = choice - 1;
            selectedVehicleTypes[selectedIndex] =
                !selectedVehicleTypes[selectedIndex];
        }
    }

    public void ChooseColor()
    {
        color = (VehicleColor?)ConsoleInput.ReadOptionalEnum(
            typeof(VehicleColor),
            "Color:");
    }

    public void ChooseNumberOfWheels()
    {
        numberOfWheels = ConsoleInput.ReadOptionalInt(
            "Number of wheels (empty = Any): ",
            minimum: 0,
            maximum: VehicleSpecification.MaximumNumberOfWheels);
    }

    public void ChooseNumberOfEngines()
    {
        numberOfEngines = ConsoleInput.ReadOptionalInt(
            "Number of engines (empty = Any): ",
            minimum: 0,
            maximum: VehicleSpecification.MaximumNumberOfEngines);
    }

    public void ChooseCylinderVolume()
    {
        cylinderVolumeCc = ConsoleInput.ReadOptionalDouble(
            "Cylinder volume in cc (empty = Any): ",
            minimum: 0,
            maximum: PistonEngineSpecification.MaximumCylinderVolumeCc);
    }

    public void ChooseFuelType()
    {
        fuelType = (FuelType?)ConsoleInput.ReadOptionalEnum(
            typeof(FuelType),
            "Fuel type:");
    }

    public void ChooseNumberOfSeats()
    {
        numberOfSeats = ConsoleInput.ReadOptionalInt(
            "Number of seats (empty = Any): ",
            minimum: 0,
            maximum: VehicleSpecification.MaximumNumberOfSeats);
    }

    public void ChooseLength()
    {
        lengthMeters = ConsoleInput.ReadOptionalDouble(
            "Length in meters (empty = Any): ",
            minimum: 0,
            maximum: VehicleSpecification.MaximumLengthMeters);
    }

    public void ChooseRegistrationNumber()
    {
        registrationNumber = ConsoleInput.ReadOptionalString(
            "Registration number (empty = Any): ");
    }

    public void ChooseManufacturer()
    {
        manufacturer = ConsoleInput.ReadOptionalString(
            "Manufacturer (empty = Any): ");
    }

    public void ChooseModel()
    {
        model = ConsoleInput.ReadOptionalString(
            "Model (empty = Any): ");
    }

    public void ChooseManufacturingYear()
    {
        manufacturingYear = ConsoleInput.ReadOptionalInt(
            "Manufacturing year (empty = Any): ",
            minimum: 1886,
            maximum: DateTime.Now.Year + 1);
    }

    public void Clear()
    {
        for (int index = 0;
             index < selectedVehicleTypes.Length;
             index++)
        {
            selectedVehicleTypes[index] = false;
        }

        color = null;
        numberOfWheels = null;
        numberOfEngines = null;
        cylinderVolumeCc = null;
        fuelType = null;
        numberOfSeats = null;
        lengthMeters = null;
        registrationNumber = null;
        manufacturer = null;
        model = null;
        manufacturingYear = null;
    }

    private Type[] CreateSelectedVehicleTypes()
    {
        int selectedCount = 0;

        for (int index = 0;
             index < selectedVehicleTypes.Length;
             index++)
        {
            if (selectedVehicleTypes[index])
            {
                selectedCount++;
            }
        }

        Type[] vehicleTypes = new Type[selectedCount];
        int resultIndex = 0;

        for (int index = 0;
             index < selectedVehicleTypes.Length;
             index++)
        {
            if (selectedVehicleTypes[index])
            {
                vehicleTypes[resultIndex] =
                    availableVehicleTypes[index];
                resultIndex++;
            }
        }

        return vehicleTypes;
    }
}
