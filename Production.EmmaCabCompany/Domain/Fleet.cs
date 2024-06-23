namespace Production.EmmaCabCompany;

public class Fleet 
{
    private readonly List<Cab> _fleet = new();
    private bool _rideRequested;
    private List<CabInfo?> _lastAssignedCab = new();
    private List<CabInfo?> _lastRideAssigned = new();

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
    public bool IsEnroute(Customer customer)
    {
        return _fleet.Any(x => x.CabInfo()?.PassengerName == customer.name);
    }
    public void RideRequested(Customer? customer)
    {
        // query to fleet from dispatcher
        if (!_fleet.Any())
        {
            throw new SystemException("There are currently no cabs in the fleet");
        }
        _rideRequested = false;
        foreach (var cab in _fleet)
        {
            if (_rideRequested) continue;

            if (!cab.IsStatus(CabStatus.Available)) continue;
            
            _rideRequested = true;
            cab.RequestRideFor(customer);
            _lastRideAssigned.Add(cab.CabInfo());
        }
        // dispatcher
        if (_rideRequested == false && customer != null)
        {
            throw new SystemException($"Dispatch failed to pickup {customer.name} as there are no available cabs.");
        }
    }
    
    public void PickupCustomer(Customer customer)
    {
        if (_fleet.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }
        foreach (var cab in _fleet)
        {
            if (!cab.IsEnrouteFor(customer))
            {
                continue;
            }
            cab.PickupAssignedCustomer(customer);
            break;
        }
    }
    
    public void DropOffCustomer()
    {
        if (_fleet.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }
        foreach (var cab in _fleet)
        {
            if (!cab.IsStatus(CabStatus.TransportingCustomer)) continue;
            _lastAssignedCab.Add(cab.CabInfo());
            cab.DropOffCustomer();
            break;
        }
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
        return _lastAssignedCab.LastOrDefault();
    }
    public CabInfo? LastRideAssigned()
    {
        return _lastRideAssigned.LastOrDefault();
    }

    public string? FindCab(Customer customer)
    {
        return _fleet.First(x => x.CabInfo()?.PassengerName == customer.name).CabInfo()?.CabName;
    }

    public bool AllCabsOccupied()
    {
        return _fleet.All(x => x.ContainsPassenger());
    }
}