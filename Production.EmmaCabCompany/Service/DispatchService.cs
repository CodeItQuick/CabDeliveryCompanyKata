namespace Production.EmmaCabCompany.Service;

public class DispatchService
{
    private readonly Dispatch _dispatch;

    public DispatchService(Dispatch dispatch)
    {
        _dispatch = dispatch;
    }

    public string AddCab()
    {
        var cabName = "Evan's Cab";
        var addCab = _dispatch.AddCab(new Cab(cabName, 20));
        if (addCab)
        {
            return "Added Evan's Cab to fleet";
        }

        return "Failed to add Evan's Cab to fleet.";
    }

    public List<string> CabNotifiesDroppedOff(List<Customer> customersPickedUp)
    {
        var list = new List<string>();
        if (customersPickedUp.Count == 0)
        {
            return ["There are currently no customer's assigned to cabs."];
        }

        var droppedOffCustomers = _dispatch.DropOffCustomers();
        foreach (var cabInfo in droppedOffCustomers)
        {
            list.Add($"{cabInfo.CabName} dropped off {cabInfo.PassengerName} at {cabInfo.Destination}.");
        }
        customersPickedUp = new List<Customer>();

        return list;
    }

    public string CabNotifiesPickedUp(
        List<Customer> customersAwaitingPickup, 
        List<Customer> customersPickedUp)
    {
        if (_dispatch.NoCabsInFleet())
        {
            return "There are currently no cabs in the fleet.";
        }

        if (customersAwaitingPickup.Count == 0)
        {
            return "There are currently no customer's assigned to cabs.";
        }
        var customer = customersAwaitingPickup.FirstOrDefault();
        _dispatch.PickupCustomer(customer);
        customersPickedUp.Add(customer);
        customersAwaitingPickup.RemoveAt(0);
        return "Notified dispatcher of pickup";
    }

    public string CustomerCabCall(
        List<string> customerNames, 
        ref int numCustomersServed, 
        List<Customer> customersCallInProgress)
    {
        var customerName = customerNames[numCustomersServed];
        numCustomersServed++;
        var customer = new Customer(customerName, "1 Fulton Drive", "1 Destination Lane");
        customersCallInProgress.Add(customer);
        return $"Received customer ride request from {customerName}";
    }

    public List<string> CustomerCancelledCabRide(
        List<Customer> customersAwaitingPickup, 
        List<Customer> customersPickedUp)
    {
        var list = new List<string>();
        if (customersAwaitingPickup.Any())
        {
            customersAwaitingPickup.RemoveAt(customersAwaitingPickup.Count - 1);
            list.Add("Customer cancelled request before cab assigned.");
            
        }

        if (!customersPickedUp.Any())
        {
            return list;
        }
        customersPickedUp.RemoveAt(customersPickedUp.Count - 1);
        list.Add("Customer cancelled request before cab got there.");

        return list;
    }

    public string RemoveCab()
    {
        if (!_dispatch.NoCabsInFleet())
        {
            var success = _dispatch.RemoveCab();
            if (success)
            {
                return "Last cab removed from cab fleet.";
            }

            return "Cab cannot be removed until passenger dropped off.";
        }

        return "No cabs in fleet currently";
    }

    public List<string> SendCabRequest(
        List<Customer> customersCallInProgress,
        List<Customer> customersAwaitingPickup)
    {
        if (_dispatch.NoCabsInFleet())
        {
            return ["There are currently no cabs in the fleet."];
        }
        if (customersCallInProgress.Any())
        {
            try
            {
                var customer = customersCallInProgress.Skip(0).First();
                var cabInfo = _dispatch.RideRequest(customer);
                if (cabInfo != null)
                {
                    customersAwaitingPickup.Add(customer);
                    customersCallInProgress.RemoveAt(0);
                    return
                    [
                        $"{cabInfo.CabName} picked up {cabInfo.PassengerName} at {cabInfo.StartLocation}.",
                        "Cab assigned to customer."
                    ];
                }
                customersAwaitingPickup.Add(customer);
                customersCallInProgress.RemoveAt(0);
                return ["Cab assigned to customer."];
            }
            catch (SystemException ex)
            {
                return [ex.Message];
            }
        }

        return ["There are currently no customer's waiting for cabs."];
    }
}