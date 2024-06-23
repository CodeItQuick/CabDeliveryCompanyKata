using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class DispatchController
{
    private readonly Dispatch _dispatch;
    private List<Customer> _customersPickedUp;
    private readonly List<Customer> _customersCallInProgress;

    public DispatchController(Dispatch dispatch)
    {
        _dispatch = dispatch;
        _customersPickedUp = new List<Customer>();
        _customersCallInProgress = new List<Customer>();
    }

    public string AddCab()
    {
        var cabName = "Evan's Cab";
        _dispatch.AddCab(new Cab(cabName, 20));
        
        return "Added Evan's Cab to fleet";
    }

    public string RemoveCab()
    {
        if (!_dispatch.NoCabsInFleet())
        {
            try
            {
                _dispatch.RemoveCab();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
            return "Last cab removed from cab fleet.";
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
        if (!_dispatch.CustomerAwaitingPickup())
        {
            return list;
        }
        _dispatch.CancelPickup();
        list.Add("Customer cancelled request before cab assigned.");

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
                _dispatch.RideRequest(customer);
                var cabInfo = _dispatch.FindEnroutePassenger(customer);
                if (cabInfo != null)
                {
                    _customersCallInProgress.RemoveAt(0);
                    return
                    [
                        $"{cabInfo.CabName} picked up {cabInfo.PassengerName} at {cabInfo.StartLocation}.",
                        "Cab assigned to customer."
                    ];
                }
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

        if (!_dispatch.CustomerAwaitingPickup())
        {
            return "There are currently no customer's assigned to cabs.";
        }
        _dispatch.PickupCustomer();
        return "Notified dispatcher of pickup";
    }

    public List<string> CabNotifiesDroppedOff()
    {
        var list = new List<string>();
        if (!_dispatch.CustomersStillInTransport())
        {
            return ["There are currently no customer's assigned to cabs."];
        }

        _dispatch.DropOffCustomer();
        // foreach (var cabInfo in droppedOffCustomers)
        // {
        var droppedOff = _dispatch.DroppedOffCustomers();
        foreach (var cabInfo in droppedOff)
        {
            list.Add($"{cabInfo.CabName} dropped off {cabInfo.PassengerName} at {cabInfo.Destination}.");
        }
        // }
        _customersPickedUp = new List<Customer>();

        return list;
    }
}