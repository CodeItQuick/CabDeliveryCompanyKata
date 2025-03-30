using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

// Aggregate Root Id
[PrimaryKey("Id")]
[Table("Fleet")]
public class Fleet
{
    public int? Id { get; set; }
    [ForeignKey("Cab")] public List<CabDriver> FleetOfCabs { get; set; } = new();
}