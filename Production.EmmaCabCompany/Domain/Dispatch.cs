namespace Production.EmmaCabCompany.Domain;

public class Dispatch 
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
        _fleet.RemoveCab();
    }

    public void RideRequest(Customer? customer)
    {
        _fleet.RideRequested(customer);
        if (customer != null && _fleet.LastRideAssigned()?.PassengerName == customer.name)
        {
            _customerStatusMap.Add(customer, CustomerStatus.WaitingPickup);
        }
    }

    public CabInfo? FindEnroutePassenger(Customer customer)
    {
        
        if (_fleet.IsEnroute(customer))
        {
            return new CabInfo()
            {
                PassengerName = customer.name,
                CabName = _fleet.FindCab(customer),
                StartLocation = customer.startLocation,
                Destination = customer.endLocation
            };
        }

        return null;
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
        _fleet.DropOffCustomer();
        var lastDroppedOff = _fleet.LastAssigned();
        Customer? lastCustomer = null; 
        if (lastDroppedOff != null)
        {
            lastCustomer = new Customer(lastDroppedOff.PassengerName, lastDroppedOff.StartLocation, lastDroppedOff.Destination);
        }
        if (lastCustomer != null && _fleet.IsEnroute(lastCustomer))
        {
            _customerStatusMap[lastCustomer] = CustomerStatus.Delivered;
        }
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
        return new List<CabInfo?>() { _fleet.LastAssigned() };
    }

    public void CancelPickup()
    {
        if (_customerStatusMap
            .Any(x => x.Value == CustomerStatus.WaitingPickup))
        {
            var customer = _customerStatusMap.FirstOrDefault().Key;
            _customerStatusMap.Remove(customer);
        }
    }
    public bool CustomerAwaitingPickup()
    {
        return _customerStatusMap
            .Any(x => x.Value == CustomerStatus.WaitingPickup);
    }
    
}

internal enum CustomerStatus
{
    WaitingPickup,
    Enroute,
    Delivered
}