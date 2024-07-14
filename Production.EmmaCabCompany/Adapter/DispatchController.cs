using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class DispatchController(RadioFleet radioFleet)
{
    private int _currentNameIdx = 0;
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

    public string CustomerCabCall()
    {
        var customerName = new List<string>()
        {
            "Emma",
            "Lisa",
            "Dan",
            "Evan",
            "Darrell",
            "Diane",
            "Bob",
            "Arlo"
        };
        radioFleet.CustomerCabCall(customerName);
        return $"Received customer ride request from {customerName[_currentNameIdx++]}";
    }

    public List<string> CustomerCancelledCabRide()
    {
        try
        {
            radioFleet.CancelPickup();
            return ["Customer cancelled cab ride successfully."];
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
        try
        {
            radioFleet.PickupCustomer();
            return "Notified dispatcher of pickup";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
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

        return list;
    }
}