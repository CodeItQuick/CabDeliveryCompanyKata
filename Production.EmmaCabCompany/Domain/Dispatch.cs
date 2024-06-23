namespace Production.EmmaCabCompany.Domain;

public class Dispatch 
{
    
    private readonly Fleet _fleet = new();

    public bool AddCab(Cab cab)
    {
        _fleet.AddCab(cab);
        return true;
    }

    public bool RemoveCab()
    {
        _fleet.RemoveCab();

        return false;
    }

    public CabInfo? RideRequest(Customer? customer)
    {
        _fleet.RideRequested(customer);

        return _fleet.LastAssigned();
    }

    public bool PickupCustomer(Customer customer)
    {
        _fleet.PickupCustomer(customer);
        // for (int i = 0; i < _fleet.Count; i++)
        // {
        //     _fleet[i].PickupAssignedCustomer(customer);
        // }

        return customer.IsPickedUp();
    }

    public List<CabInfo> DropOffCustomer()
    {
        var dropOffCustomer = _fleet.DropOffCustomer();

        return dropOffCustomer;
    }

    public bool NoCabsInFleet()
    {
        return _fleet.NoCabsInFleet();
    }
    public bool CustomersStillInTransport()
    {
        return _fleet.CustomersStillInTransport();
    }
}