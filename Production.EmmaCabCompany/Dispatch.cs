namespace Production.EmmaCabCompany;

public class Dispatch 
{
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private List<ICabs> _fleet = new List<ICabs>();

    public Dispatch(ICabCompanyPrinter cabCompanyPrinter)
    {
        _cabCompanyPrinter = cabCompanyPrinter;
    }

    // Dispatcher?
    public void AddCab(ICabs cab)
    {
        _fleet.Add(cab);
    }

    public void CallCab(Customer customer)
    {
        var success = _fleet.FirstOrDefault()?.PickupCustomer(customer);
        if (success == true)
        {
            _fleet.FirstOrDefault()?.DropOffCustomer();
        }
    }

    public void RideRequest(Customer customer)
    {
        _fleet.FirstOrDefault()?.RideRequest(customer);
    }
}