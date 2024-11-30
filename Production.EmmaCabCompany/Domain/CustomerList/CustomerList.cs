using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Domain.CustomerList;

// Aggregate Root Id
public class CustomerList
{
    public int Id { get; set; } = 1;
    public virtual List<Customer> Customers { get; set; }

    // private Dictionary<Customer, CustomerStatus> _customerStatusMap = new();

    public CustomerList()
    {
        Customers = new();
    }

    public static CustomerList CreateCustomerList(int id, List<Customer> customers)
    {
        return new CustomerList() { Id = id, Customers = customers};
    }

    public void CustomerCabCall(Customer customer)
    {
        // _customerStatusMap.Add(customer, CustomerStatus.CustomerCallInProgress);
        customer.Status = CustomerStatus.CustomerCallInProgress;
        Customers.Add(customer);
    }

    public Customer? FindRideRequestedCustomer()
    {
        if (Customers.All(x => x.Status != CustomerStatus.CustomerCallInProgress))
        {
            throw new SystemException("There are currently no customer's waiting for cabs.");
        }
        return Customers
            .FirstOrDefault(x => x.Status == CustomerStatus.CustomerCallInProgress);
    }
    // TODO: Write Aggregate Root Tests
    public void RideRequest()
    {
        // TODO: get rid of second if condition
        if (Customers.Any(x => x.Status == CustomerStatus.CustomerCallInProgress))
        {
            Customers.FirstOrDefault(x => x.Status == CustomerStatus.CustomerCallInProgress)!.Status = 
                CustomerStatus.WaitingPickup;
            return;
        }
        // TODO: get rid of below
        // if (_customerStatusMap.All(x => x.Value != CustomerStatus.CustomerCallInProgress))
        // {
        //     throw new SystemException("There are currently no customer's waiting for cabs.");
        // }
        // var customer = _customerStatusMap
        //     .FirstOrDefault(x => x.Value == CustomerStatus.CustomerCallInProgress)
        //     .Key;
        // _customerStatusMap[customer] = CustomerStatus.WaitingPickup;
    }
    public Customer? FindEnroutePassenger(CustomerStatus customerStatus)
    {
        return Customers
            .LastOrDefault(x => x.Status == customerStatus);
    }
    
    public Customer PickupCustomer()
    {
        if (Customers.All(x => x.Status != CustomerStatus.WaitingPickup))
        {
            throw new SystemException("There are currently no customer's assigned to cabs.");
        }

        if (Customers.Any(x => x.Status == CustomerStatus.WaitingPickup))
        {
            var customerToChange = Customers.FirstOrDefault(x => x.Status == CustomerStatus.WaitingPickup);
            customerToChange!.Status = CustomerStatus.Enroute;
            return customerToChange;
        }
        return Customers.FirstOrDefault(x => x.Status == CustomerStatus.WaitingPickup)!;
    }

    public void PutCustomerInRoute(Customer firstCustomer)
    {
        // TODO: move throws to this method, not the query
        var customer = Customers.FirstOrDefault(x => x.Id == firstCustomer.Id);
        if (customer != null)
        {
            customer.Status = CustomerStatus.Enroute;
        }
    }
    public void PutCustomerEnroute()
    {
        if (Customers.Any(x => x.Status == CustomerStatus.WaitingPickup))
        {
            var customerWaiting = Customers.FirstOrDefault(x => x.Status == CustomerStatus.WaitingPickup)!;
            customerWaiting.Status = CustomerStatus.Enroute;
        }
    }
    
    public void ValidateCanDropOffCustomer()
    {
        if (Customers
                .FirstOrDefault(x => x.Status == CustomerStatus.Enroute)
                == null)
        {
            throw new SystemException("No customer to drop off.");
        }
    }
    public void MarkCustomerAsDelivered()
    {
        var customer = Customers
            .FirstOrDefault(x => x.Status == CustomerStatus.Enroute);
        if (customer != null)
        {
            customer.Status = CustomerStatus.Delivered;
        }
    }
    
    public void CancelPickup()
    {
        if (Customers.All(x => 
                x.Status != CustomerStatus.WaitingPickup && x.Status != CustomerStatus.CustomerCallInProgress))
        {
            throw new SystemException("No customers are waiting for pickup. Cannot cancel cab.");
        }

        if (Customers.Any(x => x.Status == CustomerStatus.CustomerCallInProgress))
        {
            var customerToChange = Customers.FirstOrDefault(x => x.Status == CustomerStatus.CustomerCallInProgress);
            customerToChange!.Status = CustomerStatus.CancelledCall;
            return;
        }
        // var customer = _customerStatusMap.FirstOrDefault(x => 
        //     x.Value is CustomerStatus.WaitingPickup or CustomerStatus.CustomerCallInProgress).Key;
        // _customerStatusMap[customer] = CustomerStatus.CancelledCall;
    }
    
    public bool CustomerInState(CustomerStatus customerStatus)
    {
        return Customers
            .Any(x => x.Status == customerStatus);
    }

    public Dictionary<Customer, CustomerStatus> Export()
    {
        Dictionary<Customer, CustomerStatus> customerDictionary = new Dictionary<Customer, CustomerStatus>();
        Customers.ForEach(x =>
        {
            customerDictionary.Add(x, x.Status);
        });
        return customerDictionary;
    }

    public void Rebuild(Dictionary<Customer, CustomerStatus> customerDictionary)
    {
        // _customerStatusMap = customerDictionary;
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

    public void PutCustomerDelivered()
    {
        if (Customers.Any(x => x.Status == CustomerStatus.Enroute))
        {
            var customerWaiting = Customers.FirstOrDefault(x => x.Status == CustomerStatus.Enroute)!;
            customerWaiting.Status = CustomerStatus.Delivered;
        }
    }
}