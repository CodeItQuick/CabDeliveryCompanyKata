namespace Production.EmmaCabCompany;

public class Cabs : ICabs
{
    private readonly string _cabName;
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private Customer? _currentCustomer = null;
    public int Wallet { get; set; }


    public Cabs(string cabName, ICabCompanyPrinter cabCompanyPrinter, int wallet)
    {
        _cabName = cabName;
        _cabCompanyPrinter = cabCompanyPrinter;
        Wallet = wallet;
    }

    public void PickupCustomer(Customer customer)
    {
        if (customer.IsInCab || _currentCustomer != null)
        {
            return;
        }
        customer.IsInCab = true;
        customer.CabName = _cabName;
        customer.RideHistory.NameOfCabsTaken.Add(_cabName);
        _currentCustomer = customer;
        _cabCompanyPrinter.WriteLine(
            _cabName + $" picked up {customer.CustomerName} at {customer.StartLocation}");
    }

    public void DropOffCustomer(Customer customer)
    {
        if (customer.IsInCab == true && _cabName == customer.CabName)
        {
            const int cabFare = 5;
            customer.Wallet -= cabFare;
            Wallet += cabFare;
            customer.IsInCab = false;
            _currentCustomer = null;
            _cabCompanyPrinter.WriteLine(
                _cabName + $" dropped off {customer.CustomerName} at {customer.EndLocation}");
        }
    }
}