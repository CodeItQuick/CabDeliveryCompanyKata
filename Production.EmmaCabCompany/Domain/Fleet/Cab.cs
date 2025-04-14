using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Domain.Fleet;

public class Cab
{
    public int? Id { get; init; }
    public string? _cabName { get; set; }
    public  int _wallet { get; set; }
    private CabStatus _status = CabStatus.Available;
    private Patron? _assignedPassenger;
    public double _latitude { get; set; }
    public double _longitude { get; set; }
    public int CustomerId { get; set; } = 1;

    public Cab(string? cabName, int wallet, double latitude, double longitude)
    {
        _cabName = cabName;
        _wallet = wallet;
        _latitude = latitude;
        _longitude = longitude;
    }

    public bool RequestRideFor(Patron? customer)
    {
        if (_status != CabStatus.Available || _assignedPassenger != null)
        {
            return false;
        }
        _assignedPassenger = customer; // we're picking up this customer
        _status = CabStatus.CustomerRideRequested;
        return true;
    }

    public bool PickupAssignedCustomer(Patron patron)
    {
        if (!IsEnrouteFor(patron)) return false;
        _status = CabStatus.TransportingCustomer;
        return true;
    }

    public bool IsEnrouteFor(Patron patron)
    {
        return _status == CabStatus.CustomerRideRequested && 
               patron.Name == _assignedPassenger?.Name;
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

    public (double, double) CurrentLocation()
    {
        return (_latitude, _longitude);
    }
}