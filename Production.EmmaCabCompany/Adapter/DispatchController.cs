using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class DispatchController(DispatcherCoordinator dispatcherCoordinator)
{
    private int _currentNameIdx = 0;
    public string AddCab()
    {
        var cabName = "Evan's Cab";
        dispatcherCoordinator.AddCab(new Cab(cabName, 20));
        
        return "Added Evan's Cab to fleet";
    }

    public string RemoveCab()
    {
        try
        {
            dispatcherCoordinator.RemoveCab();
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
        dispatcherCoordinator.CustomerCabCall(customerName[_currentNameIdx]);
        return $"Received customer ride request from {customerName[_currentNameIdx++]}";
    }

    public List<string> CustomerCancelledCabRide()
    {
        try
        {
            dispatcherCoordinator.CancelPickup();
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
            dispatcherCoordinator.RideRequest();

            var cabInfo = dispatcherCoordinator.FindEnroutePassenger(CustomerStatus.WaitingPickup);
            
            return
            [
                $"{cabInfo?.CabName} picked up {cabInfo?.PassengerName} at {cabInfo?.StartLocation}.",
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
            dispatcherCoordinator.PickupCustomer();
            return "Notified dispatcher of pickup";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public List<string> CabNotifiesDroppedOff()
    {
        try
        {
            dispatcherCoordinator.DropOffCustomer();
            var droppedOff = dispatcherCoordinator.DroppedOffCustomer();

            return [$"{droppedOff[0]?.CabName} dropped off {droppedOff[0]?.PassengerName} at {droppedOff[0]?.Destination}."];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }
}