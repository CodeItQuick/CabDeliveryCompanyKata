namespace Production.EmmaCabCompany.Domain.CustomerList;

public class CustomerList
{
    public int Id { get; private init; } = 1;
    public List<Customer> Customers { get; init; } = new();

    public static CustomerList CreateCustomerList(int id, List<Customer> customers)
    {
        return new CustomerList() { Id = id, Customers = customers};
    }

    public void CustomerCabCall(Customer customer)
    {
        customer.Status = CustomerStatus.CustomerCallInProgress;
        Customers.Add(customer);
    }
    public void RideRequest()
    {
        if (Customers.All(x => x.Status != CustomerStatus.CustomerCallInProgress))
        {
            return;
        }
        
        Customers.FirstOrDefault(x => x.Status == CustomerStatus.CustomerCallInProgress)!.Status = 
            CustomerStatus.WaitingPickup;
    }

    public void PickupCustomer()
    {
        if (Customers.Any(x => x.Status == CustomerStatus.WaitingPickup))
        {
            var customerWaiting = Customers.FirstOrDefault(x => x.Status == CustomerStatus.WaitingPickup)!;
            customerWaiting.Status = CustomerStatus.Enroute;
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
        }
    }

    public void CustomerDelivered()
    {
        if (Customers.All(x => x.Status != CustomerStatus.Enroute))
        {
            return;
        }
        
        var customerWaiting = Customers.FirstOrDefault(x => x.Status == CustomerStatus.Enroute)!;
        customerWaiting.Status = CustomerStatus.Delivered;
    }
}