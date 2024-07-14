namespace Production.EmmaCabCompany.Domain;

public class CustomerList
{
    private Dictionary<Customer, CustomerStatus> _customerStatusMap = new(); // TODO: move this into its own class?

    // TODO: get rid of customerNames as this is not the correct behaviour
    public void CustomerCabCall(string customerCallInName)
    {
        var customer = new Customer(customerCallInName, "1 Fulton Drive", "1 Destination Lane");
        _customerStatusMap.Add(customer, CustomerStatus.CustomerCallInProgress);
    }

    public Customer? RideRequestedCustomer()
    {
        if (_customerStatusMap.All(x => x.Value != CustomerStatus.CustomerCallInProgress))
        {
            throw new SystemException("There are currently no customer's waiting for cabs.");
        }
        return _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.CustomerCallInProgress)
            .Key;
    }
    
    public void RideRequest()
    {
        if (_customerStatusMap.All(x => x.Value != CustomerStatus.CustomerCallInProgress))
        {
            throw new SystemException("There are currently no customer's waiting for cabs.");
        }
        var customer = _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.CustomerCallInProgress)
            .Key;
        _customerStatusMap[customer] = CustomerStatus.WaitingPickup;
    }
    public Customer? FindEnroutePassenger(CustomerStatus customerStatus)
    {
        return _customerStatusMap
            .LastOrDefault(x => x.Value == customerStatus)
            .Key;
    }
    
    public Customer PickupCustomer()
    {
        if (_customerStatusMap.All(x => x.Value != CustomerStatus.WaitingPickup))
        {
            throw new SystemException("There are currently no customer's assigned to cabs.");
        }
        if (_customerStatusMap.All(x => x.Value != CustomerStatus.WaitingPickup))
        {
            throw new SystemException("No customers currently waiting pickup");
        }
        var firstCustomer = _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.WaitingPickup)
            .Key;
        return firstCustomer;
    }

    public void PutCustomerInRoute(Customer firstCustomer)
    {
        // TODO: move throws to this method, not the query
        _customerStatusMap[firstCustomer] = CustomerStatus.Enroute;
    }
    
    public void ValidateCanDropOffCustomer()
    {
        if (_customerStatusMap
                .FirstOrDefault(x => x.Value == CustomerStatus.Enroute)
                .Key == null)
        {
            throw new SystemException("No customer to drop off.");
        }
    }
    public void MarkCustomerAsDelivered()
    {
        var customer = _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.Enroute)
            .Key;
        _customerStatusMap[customer] = CustomerStatus.Delivered;
    }
    
    public void CancelPickup()
    {
        if (_customerStatusMap.All(x => 
                x.Value != CustomerStatus.WaitingPickup && x.Value != CustomerStatus.CustomerCallInProgress))
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

    public Customer PickedUpCustomer()
    {
        throw new NotImplementedException();
    }
}