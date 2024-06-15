namespace Production.EmmaCabCompany;

public class Cabs : ICabs
{
    private readonly string _cabName;
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private readonly int _wallet;
    private CabStatus _status = CabStatus.Available;
    private Customer _customer;

    public Cabs(string cabName, ICabCompanyPrinter cabCompanyPrinter, int wallet)
    {
        _cabName = cabName;
        _cabCompanyPrinter = cabCompanyPrinter;
        _wallet = wallet;
    }

    public bool PickupCustomer(Customer customer)
    {
        if (_status != CabStatus.Available)
        {
            return false;
        }
        _status = CabStatus.TransportingCustomer;
        _customer = customer;
        _cabCompanyPrinter.WriteLine($"Evan's Cab picked up {customer.name} at {customer.startLocation}.");
        return true;
    }

    public bool DropOffCustomer()
    {
        if (_status != CabStatus.TransportingCustomer)
        {
            return false;
        }
        _cabCompanyPrinter.WriteLine($"Evan's Cab dropped off {_customer.name} at {_customer.endLocation}.");
        _status = CabStatus.Available;
        return true;
    }

    public bool RideRequest()
    {
        return _status == CabStatus.Available;
    }
}

internal enum CabStatus
{
    Available,
    TransportingCustomer
}