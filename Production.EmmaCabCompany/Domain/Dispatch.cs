namespace Production.EmmaCabCompany;

public class Dispatch 
{
    private readonly List<ICabs> _fleet = new();
    private bool _rideRequested;
    private CabInfo? _passenger;

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
        // query to fleet from dispatcher
        if (!_fleet.Any())
        {
            throw new SystemException("There are currently no cabs in the fleet");
        }
        // fleet and dispatch
        // fleet.AssignAvailableCab();
        AssignAvailableCab(customer);

        // dispatcher
        if (_rideRequested == false && customer != null)
        {
            throw new SystemException($"Dispatch failed to pickup {customer.name} as there are no available cabs.");
        }

        return _passenger;
    }

    public void AssignAvailableCab(Customer? customer)
    {
        _rideRequested = false;
        _passenger = null;
        for (int i = 0; i < _fleet.Count; i++)
        {
            if (_rideRequested)
            {
                continue;
            }
            
            if (_fleet[i].IsStatus(CabStatus.Available))
            {
                _rideRequested = true;
                _fleet[i].RequestRideFor(customer);
                _passenger = _fleet[i].CabInfo();
            }
        }
    }

    public bool PickupCustomer(Customer customer)
    {
        for (int i = 0; i < _fleet.Count; i++)
        {
            _fleet[i].PickupCustomer(customer);
        }

        return customer.IsPickedUp();
    }

    public List<CabInfo> DropOffCustomer()
    {
        if (_fleet.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }
        List<CabInfo> allPickedUp = new List<CabInfo>();
        for (int i = 0; i < _fleet.Count; i++)
        {
            var droppedOffCustomerSuccess = _fleet[i].IsEnroute();
            if (droppedOffCustomerSuccess)
            {
                var cabInfo = _fleet[i].CabInfo();
                _fleet[i].DropOffCustomer();
                allPickedUp.Add(cabInfo);
                break;
            }
        }

        return allPickedUp;
    }

    public bool NoCabsInFleet()
    {
        return !_fleet.Any();
    }
    public bool CustomersStillInTransport()
    {
        return _fleet.Any(x => x.ContainsPassenger());
    }
}