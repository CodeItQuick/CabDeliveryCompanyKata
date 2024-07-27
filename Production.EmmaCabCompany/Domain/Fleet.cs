namespace Production.EmmaCabCompany.Domain;

public class Fleet 
{
    private List<Cab> _fleet = new();
    private bool _rideRequested;
    
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
        return _fleet.Any(x => x.CabInfo()?.PassengerName == customer.Name);
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
        }
        // dispatcher
        if (_rideRequested == false && customer != null)
        {
            throw new SystemException($"Dispatch failed to pickup {customer.Name} as there are no available cabs.");
        }
    }
    
    public void PickupCustomer(Customer customer)
    {
        if (_fleet.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }
        var enrouteCab = _fleet?.FirstOrDefault(x => x.IsEnrouteFor(customer));
        enrouteCab?.PickupAssignedCustomer(customer);
    }
    
    public void DropOffCustomer()
    {
        if (_fleet.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }

        var enrouteCab = _fleet.FirstOrDefault(x => x.IsStatus(CabStatus.TransportingCustomer));
        enrouteCab?.DropOffCustomer();
    }

    public bool NoCabsInFleet()
    {
        return _fleet.Count == 0;
    }
    public bool CustomersStillInTransport()
    {
        return _fleet.Any(x => x.ContainsPassenger());
    }
    public CabInfo? LastRideAssigned()
    {
        return _fleet
            .LastOrDefault(x => x.IsStatus(CabStatus.CustomerRideRequested))
            ?.CabInfo();
    }

    public string? FindCab(Customer customer)
    {
        return _fleet.First(x => x.CabInfo()?.PassengerName == customer.Name).CabInfo()?.CabName;
    }

    public bool AllCabsOccupied()
    {
        return _fleet.All(x => x.ContainsPassenger());
    }

    public string[] ExportCabs()
    {
        return _fleet
            .Select(x => 
                $"{x.CabInfo()?.CabName}," +
                $"{x.CabInfo()?.PassengerName}," +
                $"{x.CabInfo()?.StartLocation}," +
                $"{x.CabInfo()?.Destination}")
            .ToArray();
    }

    public void RebuildCabList(List<Cab> cabStoredList)
    {
        _fleet = cabStoredList;
    }

    public static List<Cab> CreateCabState(string[] cabList)
    {
        var cabListStrings = cabList
            .Select(x => x)
            .ToList();
        List<Cab> cabStoredList = new List<Cab>();
        foreach (var cab in cabListStrings)
        {
            string?[] cabAttributes = cab.Split(",");
            if (cabAttributes.Length < 1 || string.IsNullOrWhiteSpace(cabAttributes[0])) continue;
            var cabValue = new Cab(cabAttributes[0], 20, 46.2382, 63.1311);
            if (!string.IsNullOrWhiteSpace(cabAttributes[1]))
            {
                var customer = new Customer(cabAttributes[1], cabAttributes[2], cabAttributes[3]);
                cabValue.RequestRideFor(customer);
            }
            cabStoredList.Add(cabValue);
        }

        return cabStoredList;
    }
}