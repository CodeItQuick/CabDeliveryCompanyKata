namespace Production.EmmaCabCompany;

public class Dispatch 
{
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private List<ICabs> _fleet = new();

    public Dispatch(ICabCompanyPrinter cabCompanyPrinter)
    {
        _cabCompanyPrinter = cabCompanyPrinter;
    }

    // Dispatcher?
    public void AddCab(ICabs cab)
    {
        _fleet.Add(cab);
    }

    public void PickupCustomer(Customer customer)
    {
        for (int i = 0; i < _fleet.Count; i++)
        {
            _fleet[i].PickupCustomer(customer);
        }
    }
    public void DropOffCustomers()
    {
        for (int i = 0; i < _fleet.Count; i++)
        {
            _fleet[i].DropOffCustomer();
        }
    }

    public void RideRequest(Customer? customer)
    {
        if (!_fleet.Any())
        {
            _cabCompanyPrinter.WriteLine("There are currently no cabs in the fleet.");
        }
        var rideRequested = false;
        for (int i = 0; i < _fleet.Count; i++)
        {
            if (rideRequested == false)
            {
                var rideRequest = _fleet[i].RideRequest(customer);
                if (rideRequest == true)
                {
                    rideRequested = true;
                }
            }
        }

        if (rideRequested == false && customer != null)
        {
            _cabCompanyPrinter.WriteLine($"Dispatch failed to pickup {customer.name} as there are no available cabs.");
        }
    }

    public bool NoCabsInFleet()
    {
        return !_fleet.Any();
    }

    public void RemoveCab()
    {
        if (_fleet[^1].RideInProgress())
        {
            _fleet.RemoveAt(_fleet.Count - 1);
        }
        else
        {
            _cabCompanyPrinter.WriteLine("Cab cannot be removed until passenger dropped off.");
        }
    }
}