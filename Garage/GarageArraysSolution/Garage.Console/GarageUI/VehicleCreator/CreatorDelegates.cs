using Garage.Engines;
using Garage.Vehicles;

namespace Garage.ConsoleApp.GarageUI;

public delegate Vehicle VehicleFactory();
public delegate void EngineCreatedHandler(Engine engine);

public delegate string? TextValueGetter();
public delegate void TextValueSetter(string value);

public delegate int? IntValueGetter();
public delegate void IntValueSetter(int value);

public delegate double? DoubleValueGetter();
public delegate void DoubleValueSetter(double value);

public delegate bool? BoolValueGetter();
public delegate void BoolValueSetter(bool value);
