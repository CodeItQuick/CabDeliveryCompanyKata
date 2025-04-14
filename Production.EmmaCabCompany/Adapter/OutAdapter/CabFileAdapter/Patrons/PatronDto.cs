using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;

[PrimaryKey("Id")]
[Table("Patrons")]
public class PatronDto
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
    [NotMapped] // should this exist at this layer if its "not mapped"?
    public (double, double) PickupLocation { get; init; }
    [Column("Status")]
    public PatronStatus Status { get; init; }
    [ForeignKey("Id")]
    public Customer Customer { get; set; }
}