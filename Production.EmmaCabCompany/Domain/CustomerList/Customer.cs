using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany;

// TODO: there should be a DTO on the aggregate, that then creates a Customer domain object
[Table("Customers")]
[PrimaryKey("Id")]
public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    [Column("CustomerName")]
    public string? Name { get; init; }
    [Column("StartLocation")]
    public string? StartLocation { get; init; }
    [Column("EndLocation")]
    public string? EndLocation { get; init; }
    [NotMapped]
    public (double, double) PickupLocation { get; init; }
    [Column("Status")]
    public CustomerStatus Status { get; set; }

    [Column("MenuId")] public int? MenuId { get; set; } = 1;
    [Column("CustomerId")] public int? CustomerId { get; set; } = 1;

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