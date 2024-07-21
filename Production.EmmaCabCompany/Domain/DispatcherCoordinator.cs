namespace Production.EmmaCabCompany.Domain;

public class DispatcherCoordinator
{
    private readonly Fleet _fleet = new();
    private readonly CustomerList _customerList = new();

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

    public void CustomerCabCall(string customerCallInName)
    {
        _customerList.CustomerCabCall(customerCallInName);
    }

    public void RideRequest()
    {
        if (_fleet.NoCabsInFleet())
        {
            throw new SystemException("There are currently no cabs in the fleet.");
        }
        var customer = _customerList.RideRequestedCustomer();
        _fleet.RideRequested(customer);
        if (customer != null && _fleet.LastRideAssigned()?.PassengerName == customer.name)
        {
            _customerList.RideRequest();
        }
    }
    
    public CabInfo? FindEnroutePassenger(CustomerStatus customerStatus)
    {
        var customer = _customerList.FindEnroutePassenger(customerStatus);
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
        if (_fleet.NoCabsInFleet())
        {
            throw new SystemException("There are currently no cabs in the fleet.");
        }
        var customer = _customerList.PickupCustomer();
        _fleet.PickupCustomer(customer);
        if (_fleet.IsEnroute(customer))
        {
            _customerList.PutCustomerInRoute(customer);
        }
    }

    public void DropOffCustomer()
    {
        if (!_fleet.CustomersStillInTransport())
        {
            throw new SystemException("There are currently no customer's assigned to cabs.");
        }
        _customerList.ValidateCanDropOffCustomer();
        _fleet.DropOffCustomer();
        _customerList.MarkCustomerAsDelivered();
    }

    public bool NoCabsInFleet()
    {
        return _fleet.NoCabsInFleet();
    }

    public List<CabInfo?> DroppedOffCustomer()
    {
        return [_fleet.LastAssigned()];
    }

    public void CancelPickup()
    {
        _customerList.CancelPickup();
    }

    public bool CustomerInState(CustomerStatus customerStatus)
    {
        return _customerList.CustomerInState(customerStatus);
    }

    public Dictionary<Customer, CustomerStatus> ExportCustomerList()
    {
        return _customerList.Export();
    }

}

public enum CustomerStatus
{
    WaitingPickup,
    Enroute,
    Delivered,
    CustomerCallInProgress
}