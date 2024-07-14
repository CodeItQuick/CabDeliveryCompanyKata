using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class DispatchController(RadioFleet radioFleet)
{
    public string AddCab()
    {
        var cabName = "Evan's Cab";
        radioFleet.AddCab(new Cab(cabName, 20));
        
        return "Added Evan's Cab to fleet";
    }

    public string RemoveCab()
    {
        try
        {
            radioFleet.RemoveCab();
            return "Cab removed from fleet";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string CustomerCabCall(
        List<string> customerNames, 
        ref int numCustomersServed)
    {
        var customerName = customerNames[numCustomersServed];
        numCustomersServed++;
        var customer = new Customer(customerName, "1 Fulton Drive", "1 Destination Lane");
        radioFleet.CustomerCabCall(customer);
        return $"Received customer ride request from {customerName}";
    }

    public List<string> CustomerCancelledCabRide()
    {
        try
        {
            radioFleet.CancelPickup();
            return [];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }

    public List<string> SendCabRequest()
    {
        try
        {
            radioFleet.RideRequest();

            var cabInfo = radioFleet.FindEnroutePassenger(CustomerStatus.WaitingPickup);
            
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
        if (radioFleet.NoCabsInFleet())
        {
            return "There are currently no cabs in the fleet.";
        }

        if (!radioFleet.CustomerInState(CustomerStatus.WaitingPickup))
        {
            return "There are currently no customer's assigned to cabs.";
        }
        radioFleet.PickupCustomer();
        return "Notified dispatcher of pickup";
    }

    public List<string> CabNotifiesDroppedOff()
    {
        var list = new List<string>();
        if (!radioFleet.CustomersStillInTransport())
        {
            return ["There are currently no customer's assigned to cabs."];
        }

        radioFleet.DropOffCustomer();
        var droppedOff = radioFleet.DroppedOffCustomers();
        foreach (var cabInfo in droppedOff)
        {
            list.Add($"{cabInfo.CabName} dropped off {cabInfo.PassengerName} at {cabInfo.Destination}.");
        }
        // }
        new List<Customer>();

        return list;
    }
}