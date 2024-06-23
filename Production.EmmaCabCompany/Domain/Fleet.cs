namespace Production.EmmaCabCompany;

public class Fleet 
{
    private readonly List<Cab> _fleet = new();
    private bool _rideRequested;
    private CabInfo? _lastAssignedCab;

    public void AddCab(Cab cab)
    {
        _fleet.Add(cab);
    }

    public void RemoveCab()
    {
        if (_fleet[^1].RideInProgress())
        {
            _fleet.RemoveAt(_fleet.Count - 1);
        }

    }
    public void RideRequested(Customer? customer)
    {
        // query to fleet from dispatcher
        if (!_fleet.Any())
        {
            throw new SystemException("There are currently no cabs in the fleet");
        }
        _rideRequested = false;
        _lastAssignedCab = null;
        foreach (var cab in _fleet)
        {
            if (_rideRequested) continue;

            if (!cab.IsStatus(CabStatus.Available)) continue;
            
            _rideRequested = true;
            cab.RequestRideFor(customer);
            _lastAssignedCab = cab.CabInfo();
        }
        // dispatcher
        if (_rideRequested == false && customer != null)
        {
            throw new SystemException($"Dispatch failed to pickup {customer.name} as there are no available cabs.");
        }
    }
    
    public bool PickupCustomer(Customer customer)
    {
        if (_fleet.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }
        foreach (var cab in _fleet)
        {
            cab.PickupAssignedCustomer(customer);
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
        foreach (var cab in _fleet)
        {
            if (!cab.IsEnroute()) continue;
            var cabInfo = cab.CabInfo();
            cab.DropOffCustomer();
            allPickedUp.Add(cabInfo);
            break;
        }

        return allPickedUp;
    }

    public bool NoCabsInFleet()
    {
        return _fleet.Count == 0;
    }
    public bool CustomersStillInTransport()
    {
        return _fleet.Any(x => x.ContainsPassenger());
    }
    public CabInfo? LastAssigned()
    {
        return _lastAssignedCab;
    }
}