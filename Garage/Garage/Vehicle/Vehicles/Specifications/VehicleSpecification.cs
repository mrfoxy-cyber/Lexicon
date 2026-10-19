using Garage.Engines;
using Garage.Vehicles.Components;

namespace Garage.Vehicles.Specifications;

public abstract class VehicleSpecification
{
    public required double WeightKg { get; init; }
    public required Dimensions Dimensions { get; init; }
    private Engine[] engines = [];

    public Engine[] Engines
    {
        get => [.. engines];
        init => engines = [.. value];
    }

    public void AddEngine (Engine engine)
    {
        Array.Resize(ref engines, engines.Length + 1);
        engines[^1] = engine;
    }

    public Engine? FindEngine(string serialNumber)
    {
        return Array.Find(engines, engine => engine.SerialNumber.Equals(
            serialNumber,
            StringComparison.OrdinalIgnoreCase));
    }

    public int FindEngineIndex(string serialNumber)
    {
        return Array.FindIndex(engines, engine => engine.SerialNumber.Equals(
            serialNumber,
            StringComparison.OrdinalIgnoreCase));
    }

    public bool RemoveEngine (Engine engine)
    {
        int index = FindEngineIndex(engine.SerialNumber);

        if(index ==-1)
        {
            return false;
        }

        /* same as

        Engine[] newEngines =
    new Engine[engines.Length - 1];

    Array.Copy(
        engines,
        sourceIndex: 0,
        newEngines,
        destinationIndex: 0,
        length: index);

    Array.Copy(
        engines,
        sourceIndex: index + 1,
        newEngines,
        destinationIndex: index,
        length: engines.Length - index - 1);

    engines = newEngines;

        */

        engines =
        [
            .. engines[..index],
            .. engines[(index + 1)..]
        ];

        return true;
    }
}
