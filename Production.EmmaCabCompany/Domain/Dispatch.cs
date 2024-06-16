namespace Production.EmmaCabCompany;

public class Dispatch 
{
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private readonly List<ICabs> _fleet = new();

    public Dispatch(ICabCompanyPrinter cabCompanyPrinter)
    {
        _cabCompanyPrinter = cabCompanyPrinter;
    }

    // Dispatcher?

    public bool AddCab(ICabs cab)
    {
        _fleet.Add(cab);
        return true;
    }

    public bool RemoveCab()
    {
        if (_fleet[^1].RideInProgress())
        {
            _fleet.RemoveAt(_fleet.Count - 1);
            return true;
        }

        return false;
    }

    public CabInfo? RideRequest(Customer? customer)
    {
        if (!_fleet.Any())
        {
            throw new SystemException("There are currently no cabs in the fleet");
        }
        var rideRequested = false;
        CabInfo? cabInfo = null;
        for (int i = 0; i < _fleet.Count; i++)
        {
            if (rideRequested == false)
            {
                var rideRequest = _fleet[i].RideRequest(customer);
                if (rideRequest == true)
                {
                    rideRequested = true;
                    cabInfo = _fleet[i].CabInfo();
                }
            }
        }

        if (rideRequested == false && customer != null)
        {
            throw new SystemException($"Dispatch failed to pickup {customer.name} as there are no available cabs.");
        }

        return cabInfo;
    }

    public bool PickupCustomer(Customer customer)
    {
        for (int i = 0; i < _fleet.Count; i++)
        {
            _fleet[i].PickupCustomer(customer);
        }

        return customer.IsPickedUp();
    }

    public List<CabInfo> DropOffCustomers()
    {
        if (_fleet.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }
        List<CabInfo> allPickedUp = new List<CabInfo>();
        for (int i = 0; i < _fleet.Count; i++)
        {
            var droppedOffCustomerSuccess = _fleet[i].ReachedDestination();
            if (droppedOffCustomerSuccess)
            {
                var cabInfo = _fleet[i].CabInfo();
                _fleet[i].DropOffCustomer();
                allPickedUp.Add(cabInfo);
            }
        }

        return allPickedUp;
    }

    public bool NoCabsInFleet()
    {
        return !_fleet.Any();
    }
}