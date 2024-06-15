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
        _fleet.FirstOrDefault()?.PickupCustomer(customer);
        _fleet.FirstOrDefault()?.DropOffCustomer();
    }
}