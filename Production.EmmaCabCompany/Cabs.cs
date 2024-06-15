namespace Production.EmmaCabCompany;

public class Cabs : ICabs
{
    private readonly string _cabName;
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private readonly int _wallet;
    private CabStatus _status = CabStatus.Available;

    public Cabs(string cabName, ICabCompanyPrinter cabCompanyPrinter, int wallet)
    {
        _cabName = cabName;
        _cabCompanyPrinter = cabCompanyPrinter;
        _wallet = wallet;
    }

    public void PickupCustomer(Customer customer)
    {
        _status = CabStatus.TransportingCustomer;
        _cabCompanyPrinter.WriteLine($"Evan's Cab picked up {customer.name} at {customer.startLocation}.");
    }

    public void DropOffCustomer(Customer customer)
    {
        _cabCompanyPrinter.WriteLine($"Evan's Cab dropped off {customer.name} at {customer.endLocation}.");
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