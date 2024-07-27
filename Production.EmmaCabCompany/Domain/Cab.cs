namespace Production.EmmaCabCompany;

public class Cab
{
    private readonly string? _cabName;
    private readonly int _wallet;
    private CabStatus _status = CabStatus.Available;
    private Customer? _assignedPassenger;
    private readonly double _latitude;
    private readonly double _longitude;

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
        customer.IsInCab(this);
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
        _assignedPassenger?.ExitCab();
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

    public bool ContainsPassenger()
    {
        return _assignedPassenger != null;
    }

    public (double, double) CurrentLocation()
    {
        return (_latitude, _longitude);
    }
}

public enum CabStatus
{
    Available,
    TransportingCustomer,
    CustomerRideRequested
}