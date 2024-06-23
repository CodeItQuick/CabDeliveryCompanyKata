namespace Production.EmmaCabCompany;

public class Cab : ICabs
{
    private readonly string _cabName;
    private readonly int _wallet;
    private CabStatus _status = CabStatus.Available;
    private Customer? _assignedPassenger;

    public Cab(string cabName, int wallet)
    {
        _cabName = cabName;
        _wallet = wallet;
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

    public bool IsAvailable()
    {
        return _status != CabStatus.Available || _assignedPassenger != null;
    }

    public bool PickupCustomer(Customer customer)
    {
        if (_status != CabStatus.CustomerRideRequested || customer.name != _assignedPassenger?.name)
        {
            return false;
        }
        _status = CabStatus.TransportingCustomer;
        customer.IsInCab(this);
        return true;
    }

    public bool IsStatus(CabStatus requestedStatus)
    {
        return _status == requestedStatus;
    }
    
    public bool IsEnroute()
    {
        return _status == CabStatus.TransportingCustomer;
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

    public CabInfo CabInfo()
    {
        return new CabInfo()
        {
            PassengerName = _assignedPassenger?.name,
            CabName = _cabName,
            StartLocation = _assignedPassenger?.startLocation,
            Destination = _assignedPassenger?.endLocation,
        };
    }

    public bool ContainsPassenger()
    {
        return _assignedPassenger != null;
    }
}

public enum CabStatus
{
    Available,
    TransportingCustomer,
    CustomerRideRequested
}