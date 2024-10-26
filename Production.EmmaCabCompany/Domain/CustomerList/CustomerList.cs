namespace Production.EmmaCabCompany.Domain;

public class CustomerList
{
    private Dictionary<Customer, CustomerStatus> _customerStatusMap = new();

    public void CustomerCabCall(Customer customer)
    {
        _customerStatusMap.Add(customer, CustomerStatus.CustomerCallInProgress);
    }

    public Customer? FindRideRequestedCustomer()
    {
        if (_customerStatusMap.All(x => x.Value != CustomerStatus.CustomerCallInProgress))
        {
            throw new SystemException("There are currently no customer's waiting for cabs.");
        }
        return _customerStatusMap
            .FirstOrDefault(x => x.Value == CustomerStatus.CustomerCallInProgress)
            .Key;
    }
    // TODO: Write Aggregate Root Tests
    public void CreateCustomerList(string[] customerList)
    {
        _customerStatusMap = new Dictionary<Customer, CustomerStatus>();
        foreach (var customerStr in customerList)
        {
            string?[] customerAttributes = customerStr.Split(",");
            if (customerAttributes.Length < 1 || string.IsNullOrWhiteSpace(customerAttributes[0]))
            {
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(customerAttributes[0]))
                {
                    var customer = new Customer(
                        customerAttributes[0], 
                        customerAttributes[1], 
                        customerAttributes[2]);
                    CustomerStatus.TryParse(customerAttributes[3], out CustomerStatus customerStatus);
                    _customerStatusMap.Add(customer, customerStatus);
                }
            }
        }
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
        var customer = _customerStatusMap.FirstOrDefault(x => 
            x.Value is CustomerStatus.WaitingPickup or CustomerStatus.CustomerCallInProgress).Key;
        _customerStatusMap[customer] = CustomerStatus.CancelledCall;
    }
    
    public bool CustomerInState(CustomerStatus customerStatus)
    {
        return _customerStatusMap
            .Any(x => x.Value == customerStatus);
    }

    public Dictionary<Customer, CustomerStatus> Export()
    {
        return _customerStatusMap;
    }

    public void Rebuild(Dictionary<Customer, CustomerStatus> customerDictionary)
    {
        _customerStatusMap = customerDictionary;
    }

    public static Dictionary<Customer, CustomerStatus> CreateCustomerState(string[] customerList)
    {
        Dictionary<Customer, CustomerStatus> customerDictionary = new Dictionary<Customer, CustomerStatus>();
        foreach (var customer in customerList)
        {
            string?[] customerAttribs = customer.Split(",");
            if (customerAttribs.Length <= 2) continue;
            var customerKey = new Customer(customerAttribs[0], customerAttribs[1], customerAttribs[2]);
            var customerStatus = Enum.Parse<CustomerStatus>(customerAttribs[3], true);
            customerDictionary.Add(customerKey, customerStatus);
        }

        return customerDictionary;
    }
}