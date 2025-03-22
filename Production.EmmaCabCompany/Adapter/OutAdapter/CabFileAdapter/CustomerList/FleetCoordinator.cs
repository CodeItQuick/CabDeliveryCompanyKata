using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;

[PrimaryKey("Id")]
[Table("CustomerList")]
public class FleetCoordinator : IdentityClass
{
    public override int Id { get; set; }
    [ForeignKey("CustomerId")]
    public List<PatronDto> Patrons { get; set; }
}