using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Domain.Fleet;

public class Cab
{
    public int? Id { get; set; }
    public string? _cabName { get; set; }
    public  int _wallet { get; set; }
    public CabStatus _status { get; set; } = CabStatus.Available;
    public Patron? _assignedPassenger { get; set; }
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
        if (_status != CabStatus.CustomerRideRequested) return false;
        _status = CabStatus.TransportingCustomer;
        return true;
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