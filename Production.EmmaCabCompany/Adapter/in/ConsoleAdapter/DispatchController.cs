using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Service;
using Tests.CabDeliveryCompanyKata;

namespace Production.EmmaCabCompany.Adapter.@out;

public class DispatchController
{
    private int _currentNameIdx = 0;
    private MenuService _menuService;
    private readonly CustomerListRepository _customerListRepository;
    private readonly CabServiceHandler _cabServiceHandler;
    private CustomerCabRequestedHandler _customerCabRequestedHandler;
    private CustomerRideRequestedHandler _customerRideRequestedHandler;
    private CustomerCancelledCabHandler _customerCancelledCabHandler;
    private CustomerDeliveredHandler _customerDeliveredHandler;
    private CustomerEnroutedHandler _customerEnroutedHandler;
    private CustomerPickedUpHandler _customerPickedUpHandler;

    public DispatchController(CabServiceHandler cabServiceHandler, MenuService menuService, CustomerListRepository customerListRepository)
    {
        _menuService = menuService;
        _customerListRepository = customerListRepository;
        _cabServiceHandler = cabServiceHandler;
        _customerCabRequestedHandler = new CustomerCabRequestedHandler(_customerListRepository);
        _customerCancelledCabHandler = new CustomerCancelledCabHandler(_customerListRepository);
        _customerDeliveredHandler = new CustomerDeliveredHandler(_customerListRepository);
        _customerEnroutedHandler = new CustomerEnroutedHandler(_customerListRepository);
        _customerPickedUpHandler = new CustomerPickedUpHandler(_customerListRepository);
        _customerRideRequestedHandler = new CustomerRideRequestedHandler(_customerListRepository);
    }

    public string AddCab()
    {
        var cabName = "Evan's Cab";
        _cabServiceHandler.AddCab(new Cab(cabName, 20, 46.2382, 63.1311));
        
        return "Added Evan's Cab to fleet";
    }
    public string RemoveCab()
    {
        try
        {
            _cabServiceHandler.RemoveCab();
            return "Cab removed from fleet";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string CustomerCabCall(string? customerName, string? startLocation, string? destinationLane)
    {
        var customerCabCall = _cabServiceHandler.CustomerCabCall(customerName, startLocation, destinationLane);
        var resultText = $"Received customer ride request from {customerCabCall}";
        _currentNameIdx += 1;
        _customerCabRequestedHandler.Handle(new CustomerCabRequested(customerName, startLocation, destinationLane));
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
            _cabServiceHandler.CancelPickup();
            _customerCancelledCabHandler.Handle(new CustomerCancelledCab());
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
            var response = _cabServiceHandler.SendCabRequest();
            _customerRideRequestedHandler.Handle(new CustomerRideRequested());
            
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
            _cabServiceHandler.PickupCustomer();
            _customerPickedUpHandler.Handle(new CustomerPickedUp());
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
            var droppedOff = _cabServiceHandler.DropOffCustomer();
            _customerDeliveredHandler.Handle(new CustomerDelivered());
            return [$"{droppedOff[0]?.CabName} dropped off {droppedOff[0]?.PassengerName} at {droppedOff[0]?.Destination}."];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }
}