using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class DispatchController(DispatcherCoordinator dispatcherCoordinator, CabService cabService)
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
        var customerCabCall = cabService.CustomerCabCall(customerName[_currentNameIdx]);
        var resultText = $"Received customer ride request from {customerCabCall}";
        _currentNameIdx += 1;
        return resultText;
    }
    public List<string> CustomerCancelledCabRide()
    {
        try
        {
            cabService.CancelPickup();
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
            var response = cabService.SendCabRequest();
            return response.ToList();
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
            cabService.PickupCustomer();
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
            var droppedOff = cabService.DropOffCustomer();
            return [$"{droppedOff[0]?.CabName} dropped off {droppedOff[0]?.PassengerName} at {droppedOff[0]?.Destination}."];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }
}