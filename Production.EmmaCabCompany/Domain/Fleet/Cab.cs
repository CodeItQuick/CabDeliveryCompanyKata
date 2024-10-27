using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany;

[Table("Cabs")]
[PrimaryKey("Id")]
public class Cab
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    [Column("CabName")]
    public string? _cabName { get; set; }
    [Column("Wallet")]
    public  int _wallet { get; set; }
    private CabStatus _status = CabStatus.Available;
    private Customer? _assignedPassenger;
    [Column("Latitude")]
    public double _latitude { get; set; }
    [Column("Longitude")]
    public double _longitude { get; set; }
    
    // [ForeignKey(nameof(Fleet.Id))]
    // public virtual Fleet Fleet { get; set; }

    public Cab(string? cabName, int wallet, double latitude, double longitude)
    {
        _cabName = cabName;
        _wallet = wallet;
        _latitude = latitude;
        _longitude = longitude;
    }

    public bool RequestRideFor(Customer? customer)
    {
        if (_status != CabStatus.Available || _assignedPassenger != null)
        {
            return false;
        }
        _assignedPassenger = customer; // we're picking up this customer
        _status = CabStatus.CustomerRideRequested;
        return true;
    }

    public bool PickupAssignedCustomer(Customer customer)
    {
        if (!IsEnrouteFor(customer)) return false;
        _status = CabStatus.TransportingCustomer;
        return true;
    }

    public bool IsEnrouteFor(Customer customer)
    {
        return _status == CabStatus.CustomerRideRequested && 
               customer.Name == _assignedPassenger?.Name;
    }

    public bool IsStatus(CabStatus requestedStatus)
    {
        return _status == requestedStatus;
    }

    public bool DropOffCustomer()
    {
        if (_status != CabStatus.TransportingCustomer)
        {
            return false;
        }
        _status = CabStatus.Available;
        _assignedPassenger = null;
        return _status == CabStatus.Available;
    }

    public bool RideInProgress()
    {
        return _status == CabStatus.Available;
    }
    
    public CabInfo? CabInfo()
    {
        
        return new CabInfo()
        {
            PassengerName = _assignedPassenger?.Name,
            CabName = _cabName,
            StartLocation = _assignedPassenger?.StartLocation,
            Destination = _assignedPassenger?.EndLocation,
        };
    }
    public string AssignedPassenger()
    {
        return _assignedPassenger!.Name!;
    }

    public bool ContainsPassenger()
    {
        return _assignedPassenger != null;
    }

    public (double, double) CurrentLocation()
    {
        return (_latitude, _longitude);
    }

    public bool IsCloserThan(Cab assignedCab, Customer customer)
    {
        return CalculateDistanceBetweenTwoPoints(CurrentLocation(), customer.PickupLocation) <
               CalculateDistanceBetweenTwoPoints(assignedCab.CurrentLocation(), customer.PickupLocation);
    }
    
    private static double CalculateDistanceBetweenTwoPoints((double?, double?) firstLocation, (double, double) secondLocation)
    {
        var formulaOne =
            Math.Sqrt(Math.Pow(firstLocation.Item1 ?? 0 - secondLocation.Item1, 2.0) +
                      Math.Pow(firstLocation.Item2 ?? 0 - secondLocation.Item2, 2.0));
        var formulaTwo =
            Math.Sqrt(Math.Pow(firstLocation.Item1 - secondLocation.Item1 ?? 0, 2.0) +
                      Math.Pow(firstLocation.Item2 - secondLocation.Item2 ?? 0, 2.0));
        return formulaOne < formulaTwo ? formulaOne : formulaTwo;
     
    }
}

public enum CabStatus
{
    Available,
    TransportingCustomer,
    CustomerRideRequested
}