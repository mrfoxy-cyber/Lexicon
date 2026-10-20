namespace Garage.Results;

public record GarageOperationResult(
    bool Success,
    int? SpotIndex,
    string Message);
