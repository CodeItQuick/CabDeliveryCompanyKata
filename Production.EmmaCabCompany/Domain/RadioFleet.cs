namespace Production.EmmaCabCompany.Domain;

public class RadioFleet 
{
    
    private readonly Fleet _fleet = new();
    private Dictionary<Customer, CustomerStatus> _customerStatusMap = new();

    public void AddCab(Cab cab)
    {
        _fleet.AddCab(cab);
    }

    public void RemoveCab()
    {
        if (_fleet.AllCabsOccupied())
        {
            throw new SystemException("Cab cannot be removed until passenger dropped off.");
        }
        if (_fleet.NoCabsInFleet())
        {
            throw new SystemException("Last cab removed from fleet.");
        }
        
        _fleet.RemoveCab();
    }

    public void CustomerCabCall(Customer customer)
    {
        _customerStatusMap.Add(customer, CustomerStatus.CustomerCallInProgress);
    }

    public void RideRequest()
    {
        if (!CustomerInState(CustomerStatus.CustomerCallInProgress))
        {
            throw new SystemException("No customer waiting for a ride");
        }
        var customer = _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.CustomerCallInProgress)
            .Key;
        _fleet.RideRequested(customer);
        if (customer != null && _fleet.LastRideAssigned()?.PassengerName == customer.name)
        {
            _customerStatusMap[customer] = CustomerStatus.WaitingPickup;
        }
    }

    public CabInfo? FindEnroutePassenger(CustomerStatus customerStatus)
    {
        var customer = _customerStatusMap
            .LastOrDefault(x => x.Value == customerStatus)
            .Key;
        if (customer == null)
        {
            return null;
        }
        var findCab = _fleet.FindCab(customer);
        return new CabInfo()
            {
                PassengerName = customer.name,
                CabName = findCab,
                StartLocation = customer.startLocation,
                Destination = customer.endLocation
            };
    }

    public void PickupCustomer()
    {
        var hasWaitingPickup = _customerStatusMap
            .Any(x => x.Value == CustomerStatus.WaitingPickup);
        if (!hasWaitingPickup)
        {
            throw new SystemException("No customers currently waiting pickup");
        };
        var firstCustomer = _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.WaitingPickup)
            .Key;
        _fleet.PickupCustomer(firstCustomer);
        if (_fleet.IsEnroute(firstCustomer))
        {
            _customerStatusMap[firstCustomer] = CustomerStatus.Enroute;
        }
        
    }

    public void DropOffCustomer()
    {
        var lastCustomer = _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.Enroute)
            .Key;
        if (lastCustomer == null)
        {
            throw new SystemException("No customer to drop off.");
        }
        _fleet.DropOffCustomer();
        _customerStatusMap[lastCustomer] = CustomerStatus.Delivered;
    }

    public bool NoCabsInFleet()
    {
        return _fleet.NoCabsInFleet();
    }

    public bool CustomersStillInTransport()
    {
        return _fleet.CustomersStillInTransport();
    }

    public List<CabInfo?> DroppedOffCustomers()
    {
        return [_fleet.LastAssigned()];
    }

    public void CancelPickup()
    {
        if (!CustomerInState(CustomerStatus.WaitingPickup))
        {
            throw new SystemException("No customers are waiting for pickup. Cannot cancel cab.");
        }
        var customer = _customerStatusMap.FirstOrDefault().Key;
        _customerStatusMap.Remove(customer);
    }

    public bool CustomerInState(CustomerStatus customerStatus)
    {
        return _customerStatusMap
            .Any(x => x.Value == customerStatus);
    }

}

public enum CustomerStatus
{
    WaitingPickup,
    Enroute,
    Delivered,
    CustomerCallInProgress
}