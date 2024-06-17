namespace Production.EmmaCabCompany;

public class Customers
{
    private readonly List<Customer> _customersAwaitingPickup;
    private List<Customer> _customersPickedUp;
    private readonly List<Customer> _customersCallInProgress;

    public Customers()
    {
        _customersAwaitingPickup = new List<Customer>();
        _customersPickedUp = new List<Customer>();
        _customersCallInProgress = new List<Customer>();
    }

}