using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

[Table("Cabs")]
[PrimaryKey("Id")]
public class CabDriver
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    [Column("CabName")]
    public string? _cabName { get; set; }
    [Column("Wallet")]
    public  int _wallet { get; set; }
    [Column("Status")]
    private CabStatus _status = CabStatus.Available;
    private Customer? _assignedPassenger;
    [Column("Latitude")]
    public double _latitude { get; set; }
    [Column("Longitude")]
    public double _longitude { get; set; }
    
    // [ForeignKey(nameof(Fleet.Id))]
    // public virtual Fleet Fleet { get; set; }
    public CabDriver()
    {
    }

    public CabDriver(string? cabName, int wallet, double latitude, double longitude)
    {
        _cabName = cabName;
        _wallet = wallet;
        _latitude = latitude;
        _longitude = longitude;
    }

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