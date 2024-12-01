using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

[PrimaryKey("Id")]
[Table("Customers")]
public class CustomerDto
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
    public CustomerStatus Status { get; init; }

    [Column("MenuId")] public int? MenuId { get; init; } = 1;
    [Column("CustomerId")] public int? CustomerId { get; init; } = 1;
}