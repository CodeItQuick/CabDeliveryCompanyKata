using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class DispatchController
{
    private readonly RadioFleet _radioFleet;

    public DispatchController(RadioFleet radioFleet)
    {
        _radioFleet = radioFleet;
    }

    public string AddCab()
    {
        var cabName = "Evan's Cab";
        _radioFleet.AddCab(new Cab(cabName, 20));
        
        return "Added Evan's Cab to fleet";
    }

    public string RemoveCab()
    {
        if (!_radioFleet.NoCabsInFleet())
        {
            try
            {
                _radioFleet.RemoveCab();
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
        _radioFleet.CustomerCabCall(customer);
        return $"Received customer ride request from {customerName}";
    }

    public List<string> CustomerCancelledCabRide()
    {
        var list = new List<string>();
        if (!_radioFleet.CustomerInState(CustomerStatus.WaitingPickup))
        {
            return list;
        }
        _radioFleet.CancelPickup();
        list.Add("Customer cancelled request before cab assigned.");

        return list;
    }

    public List<string> SendCabRequest()
    {
        if (_radioFleet.NoCabsInFleet())
        {
            return ["There are currently no cabs in the fleet."];
        }

        if (!_radioFleet.CustomerInState(CustomerStatus.CustomerCallInProgress))
        {
            return ["There are currently no customer's waiting for cabs."];
        }
        
        try
        {
            _radioFleet.RideRequest();

            var cabInfo = _radioFleet.FindEnroutePassenger(CustomerStatus.WaitingPickup);
            
            return
            [
                $"{cabInfo.CabName} picked up {cabInfo.PassengerName} at {cabInfo.StartLocation}.",
                "Cab assigned to customer."
            ];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
        
    }

    public string CabNotifiesPickedUp()
    {
        if (_radioFleet.NoCabsInFleet())
        {
            return "There are currently no cabs in the fleet.";
        }

        if (!_radioFleet.CustomerInState(CustomerStatus.WaitingPickup))
        {
            return "There are currently no customer's assigned to cabs.";
        }
        _radioFleet.PickupCustomer();
        return "Notified dispatcher of pickup";
    }

    public List<string> CabNotifiesDroppedOff()
    {
        var list = new List<string>();
        if (!_radioFleet.CustomersStillInTransport())
        {
            return ["There are currently no customer's assigned to cabs."];
        }

        _radioFleet.DropOffCustomer();
        var droppedOff = _radioFleet.DroppedOffCustomers();
        foreach (var cabInfo in droppedOff)
        {
            list.Add($"{cabInfo.CabName} dropped off {cabInfo.PassengerName} at {cabInfo.Destination}.");
        }
        // }
        new List<Customer>();

        return list;
    }
}