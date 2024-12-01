using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

// Aggregate Root Id
[PrimaryKey("Id")]
[Table("Fleet")]
public class Fleet
{
    public int Id = 1;
    [ForeignKey("Cab")] public virtual List<CabDto> FleetOfCabs { get; set; } = new();
}