namespace Production.EmmaCabCompany.Service;

public class DispatchController
{
    private readonly Dispatch _dispatch;
    private readonly List<Customer> _customersAwaitingPickup;
    private List<Customer> _customersPickedUp;
    private readonly List<Customer> _customersCallInProgress;

    public DispatchController(Dispatch dispatch)
    {
        _dispatch = dispatch;
        _customersAwaitingPickup = new List<Customer>();
        _customersPickedUp = new List<Customer>();
        _customersCallInProgress = new List<Customer>();
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

    public string CustomerCabCall(
        List<string> customerNames, 
        ref int numCustomersServed)
    {
        var customerName = customerNames[numCustomersServed];
        numCustomersServed++;
        var customer = new Customer(customerName, "1 Fulton Drive", "1 Destination Lane");
        _customersCallInProgress.Add(customer);
        return $"Received customer ride request from {customerName}";
    }

    public List<string> CustomerCancelledCabRide()
    {
        var list = new List<string>();
        if (_customersAwaitingPickup.Any())
        {
            _customersAwaitingPickup.RemoveAt(_customersAwaitingPickup.Count - 1);
            list.Add("Customer cancelled request before cab assigned.");
        }

        if (!_customersPickedUp.Any())
        {
            return list;
        }
        _customersPickedUp.RemoveAt(_customersPickedUp.Count - 1);
        list.Add("Customer cancelled request before cab got there.");

        return list;
    }

    public List<string> SendCabRequest()
    {
        if (_dispatch.NoCabsInFleet())
        {
            return ["There are currently no cabs in the fleet."];
        }
        if (_customersCallInProgress.Any())
        {
            try
            {
                var customer = _customersCallInProgress.Skip(0).First();
                var cabInfo = _dispatch.RideRequest(customer);
                if (cabInfo != null)
                {
                    _customersAwaitingPickup.Add(customer);
                    _customersCallInProgress.RemoveAt(0);
                    return
                    [
                        $"{cabInfo.CabName} picked up {cabInfo.PassengerName} at {cabInfo.StartLocation}.",
                        "Cab assigned to customer."
                    ];
                }
                _customersAwaitingPickup.Add(customer);
                _customersCallInProgress.RemoveAt(0);
                return ["Cab assigned to customer."];
            }
            catch (SystemException ex)
            {
                return [ex.Message];
            }
        }

        return ["There are currently no customer's waiting for cabs."];
    }

    public string CabNotifiesPickedUp()
    {
        if (_dispatch.NoCabsInFleet())
        {
            return "There are currently no cabs in the fleet.";
        }

        if (_customersAwaitingPickup.Count == 0)
        {
            return "There are currently no customer's assigned to cabs.";
        }
        var customer = _customersAwaitingPickup.FirstOrDefault();
        _dispatch.PickupCustomer(customer);
        _customersPickedUp.Add(customer);
        _customersAwaitingPickup.RemoveAt(0);
        return "Notified dispatcher of pickup";
    }

    public List<string> CabNotifiesDroppedOff()
    {
        var list = new List<string>();
        if (!_dispatch.CustomersStillInTransport())
        {
            return ["There are currently no customer's assigned to cabs."];
        }

        var droppedOffCustomers = _dispatch.DropOffCustomer();
        foreach (var cabInfo in droppedOffCustomers)
        {
            list.Add($"{cabInfo.CabName} dropped off {cabInfo.PassengerName} at {cabInfo.Destination}.");
        }
        _customersPickedUp = new List<Customer>();

        return list;
    }
}