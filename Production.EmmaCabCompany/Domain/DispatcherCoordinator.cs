namespace Production.EmmaCabCompany.Domain;

public class DispatcherCoordinator
{
    private Fleet _fleet = new();
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

    public void CustomerCabCall(Customer customer)
    {
        _customerList.CustomerCabCall(customer);
    }

    public void RideRequest()
    {
        if (_fleet.NoCabsInFleet())
        {
            throw new SystemException("There are currently no cabs in the fleet.");
        }
        var customer = _customerList.FindRideRequestedCustomer();
        _fleet.RideRequested(customer);
        _customerList.RideRequest();
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
                PassengerName = customer.Name,
                CabName = findCab,
                StartLocation = customer.StartLocation,
                Destination = customer.EndLocation
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

    public string[] ExportCabList()
    {
        return _fleet.ExportCabs();
    }

    public void RebuildCustomerDictionary(Dictionary<Customer,CustomerStatus> customerDictionary)
    {
        _customerList.Rebuild(customerDictionary);
    }

    public void RebuildCabList(Fleet cabStoredList)
    {
        _fleet = cabStoredList;
    }

    public Customer? RetrieveCustomerInState(CustomerStatus enroute)
    {
        return _customerList.FindEnroutePassenger(enroute);
    }

    public List<int> MenuState()
    {
        var menuState = new List<int>();
        if (!_fleet.NoCabsInFleet() && _customerList.CustomerInState(CustomerStatus.CustomerCallInProgress))
        {
            menuState.Add(3);
            menuState.Add(6);
        }
        if (!_fleet.NoCabsInFleet() && _customerList.CustomerInState(CustomerStatus.WaitingPickup))
        {
            menuState.Add(4);
        }
        if (!_fleet.NoCabsInFleet() && _customerList.CustomerInState(CustomerStatus.Enroute))
        {
            menuState.Add(5);
        }

        return menuState;
    }
}