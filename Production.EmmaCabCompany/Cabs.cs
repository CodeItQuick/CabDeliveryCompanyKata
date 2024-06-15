namespace Production.EmmaCabCompany;

public class Cabs : ICabs
{
    private readonly string _cabName;
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private readonly int _wallet;
    private CabStatus _status = CabStatus.Available;
    private Customer? _passenger;

    public Cabs(string cabName, ICabCompanyPrinter cabCompanyPrinter, int wallet)
    {
        _cabName = cabName;
        _cabCompanyPrinter = cabCompanyPrinter;
        _wallet = wallet;
    }

    public bool PickupCustomer(Customer customer)
    {
        if (_status != CabStatus.CustomerRideRequested || customer.name != _passenger?.name)
        {
            return false;
        }
        _status = CabStatus.TransportingCustomer;
        _cabCompanyPrinter.WriteLine($"Evan's Cab picked up {customer.name} at {customer.startLocation}.");
        return true;
    }

    public bool DropOffCustomer()
    {
        if (_status != CabStatus.TransportingCustomer)
        {
            return false;
        }
        _cabCompanyPrinter.WriteLine($"Evan's Cab dropped off {_passenger.name} at {_passenger.endLocation}.");
        _status = CabStatus.Available;
        _passenger = null;
        return _status != CabStatus.Available;
    }

    public bool RideRequest(Customer customer)
    {
        if (_status == CabStatus.Available && _passenger == null)
        {
            _passenger = customer;
            _status = CabStatus.CustomerRideRequested;
            return true;
        }

        return false;
    }
}

internal enum CabStatus
{
    Available,
    TransportingCustomer,
    CustomerRideRequested
}