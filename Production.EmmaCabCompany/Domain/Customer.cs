namespace Production.EmmaCabCompany;

public class Customer
{
    public readonly string? Name;
    public readonly string? StartLocation;
    public readonly string? EndLocation;
    public (double, double) PickupLocation;

    public Customer(string? customerName, string startLocation, string? endLocation)
    {
        Name = customerName;
        StartLocation = startLocation;
        EndLocation = endLocation;
        var isAssignable = PickupAssignmentLocations.LocationCoordinates
            .TryGetValue(startLocation, out var pickupLocation);
        PickupLocation = isAssignable ? 
            pickupLocation : 
            PickupAssignmentLocations.LocationCoordinates["Summerside"];
    }
}

public static class PickupAssignmentLocations
{
    public static readonly Dictionary<string, (double, double)> LocationCoordinates = new()
    {
        ["1 Fulton Drive"] = (46.238888, -63.129166),
        ["2 Fulton Drive"] = (46.238888, -63.129166),
        ["Bowling Alley"] = (46.23496, -63.12495),
        ["Walmart"] = (46.26205, -63.15237),
        ["Summerside"] = (46.5556, 63.1311) // "Summerside", but not actually the coordinates
    };
}