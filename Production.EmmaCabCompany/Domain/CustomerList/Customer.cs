using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Domain.CustomerList;

// TODO: there should be a DTO on the aggregate, that then creates a Customer domain object
public class Customer
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? StartLocation { get; init; }
    public string? EndLocation { get; init; }
    public (double, double) PickupLocation { get; init; }
    public CustomerStatus Status { get; set; }
    public int? MenuId { get; set; } = 1;
    public int? CustomerId { get; set; } = 1;

    public Customer()
    {
    }

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