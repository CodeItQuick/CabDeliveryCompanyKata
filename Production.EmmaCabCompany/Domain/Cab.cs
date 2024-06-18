namespace Production.EmmaCabCompany;

public class Cab : ICabs
{
    private readonly string _cabName;
    private readonly int _wallet;
    private CabStatus _status = CabStatus.Available;
    private Customer? _passenger;

    public Cab(string cabName, int wallet)
    {
        _cabName = cabName;
        _wallet = wallet;
    }

    public bool RideRequest(Customer? customer)
    {
        if (_status != CabStatus.Available || _passenger != null)
        {
            return false;
        }
        _passenger = customer;
        _status = CabStatus.CustomerRideRequested;
        return true;

    }

    public bool PickupCustomer(Customer customer)
    {
        if (_status != CabStatus.CustomerRideRequested || customer.name != _passenger?.name)
        {
            return false;
        }
        _status = CabStatus.TransportingCustomer;
        customer.IsInCab(this);
        return true;
    }

    public bool ReachedDestination()
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
        _passenger?.ExitCab();
        _passenger = null;
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
            PassengerName = _passenger?.name,
            CabName = _cabName,
            StartLocation = _passenger?.startLocation,
            Destination = _passenger?.endLocation,
        };
    }

    public bool ContainsPassenger()
    {
        return _passenger != null;
    }
}

internal enum CabStatus
{
    Available,
    TransportingCustomer,
    CustomerRideRequested
}