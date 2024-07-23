using Tests.CabDeliveryCompanyKata;

namespace Production.EmmaCabCompany.Service;

public class DispatchController
{
    private int _currentNameIdx = 0;
    private MenuService _menuService;
    private readonly CabService cabService;
    public DispatchController(CabService cabService, MenuService menuService)
    {
        _menuService = menuService;
        this.cabService = cabService;
    }


    public string AddCab()
    {
        var cabName = "Evan's Cab";
        cabService.AddCab(new Cab(cabName, 20));
        
        return "Added Evan's Cab to fleet";
    }
    public string RemoveCab()
    {
        try
        {
            cabService.RemoveCab();
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
            if (!_menuService.IsValidMenuOption(6))
            {
                throw new SystemException("This is not a valid option.");
            }
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
            if (!_menuService.IsValidMenuOption(3))
            {
                throw new SystemException("This is not a valid option.");
            }
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
            if (!_menuService.IsValidMenuOption(4))
            {
                throw new SystemException("This is not a valid option.");
            }
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
            if (!_menuService.IsValidMenuOption(5))
            {
                throw new SystemException("This is not a valid option.");
            }
            var droppedOff = cabService.DropOffCustomer();
            return [$"{droppedOff[0]?.CabName} dropped off {droppedOff[0]?.PassengerName} at {droppedOff[0]?.Destination}."];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }
}