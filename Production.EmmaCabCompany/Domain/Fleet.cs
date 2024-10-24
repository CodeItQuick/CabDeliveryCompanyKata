namespace Production.EmmaCabCompany.Domain;

// Aggregate Root Id
public class Fleet 
{
    private List<Cab> _fleet = new();

    public void CreateFleet(string[] cabList)
    {
        var cabListStrings = cabList
            .Select(x => x)
            .ToList();
        _fleet = new List<Cab>();
        foreach (var cab in cabListStrings)
        {
            string?[] cabAttributes = cab.Split(",");
            if (cabAttributes.Length < 1 || string.IsNullOrWhiteSpace(cabAttributes[0]))
            {
            }
            else
            {
                var cabValue = new Cab(cabAttributes[0], 20, 46.2382, 63.1311);
                if (!string.IsNullOrWhiteSpace(cabAttributes[1]))
                {
                    var customer = new Customer(
                        cabAttributes[1], 
                        cabAttributes[2], 
                        cabAttributes[3]);
                    cabValue.RequestRideFor(customer);
                }
                _fleet.Add(cabValue);
            }
        }
    }

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

    public void RideRequested(Customer customer)
    {
        var availableCabList = _fleet
            .Where(x => x.IsStatus(CabStatus.Available))
            .ToList();
        if (availableCabList.Count == 0)
        {
            throw new SystemException($"Dispatch failed to pickup {customer.Name} as there are no available cabs.");
        }
        Cab? assignedCab = null;
        foreach (var cab in availableCabList)
        {
            if (assignedCab != null)
            {
                if (cab.IsCloserThan(assignedCab, customer))
                {
                    assignedCab = cab;
                }

                continue;
            }
            
            assignedCab = cab;
        }
        assignedCab?.RequestRideFor(customer);
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

    public Fleet()
    {
        
    }
}