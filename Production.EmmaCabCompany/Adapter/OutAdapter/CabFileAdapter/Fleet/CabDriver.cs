using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

[Table("CabDrivers")]
// [PrimaryKey("Id")]
public class CabDriver
{
    // [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    [Column("CabName")]
    public string? _cabName { get; set; }
    [Column("Wallet")]
    public  int _wallet { get; set; }
    [Column("Status")]
    public CabStatus _status { get; set; } = CabStatus.Available;
    [Column("Latitude")]
    public double _latitude { get; set; }
    [Column("Longitude")]
    public double _longitude { get; set; }
    [ForeignKey("Id")]
    public Customer? Customer { get; set; }
    
    public bool IsStatus(CabStatus requestedStatus)
    {
        return _status == requestedStatus;
    }
}

public enum CabStatus
{
    Available,
    TransportingCustomer,
    CustomerRideRequested
}