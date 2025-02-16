using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class DispatchController
{
    private readonly CustomerListRepository _customerListRepository;
    private readonly MenuRepository _menuRepository;
    private ApplicationHandler _applicationHandler;

    public DispatchController(CustomerListRepository customerListRepository, FleetRepository fleetRepository, MenuRepository menuRepository)
    {
        _customerListRepository = customerListRepository;
        _menuRepository = menuRepository;
        _applicationHandler = new ApplicationHandler(
            new CustomerCabRequestedHandler(customerListRepository),
            new CustomerCancelledCabHandler(customerListRepository),
            new CustomerPickedUpHandler(customerListRepository),
            new CustomerDeliveredHandler(customerListRepository),
            new AddCabCommandHandler(fleetRepository),
            new RemoveCabCommandHandler(fleetRepository),
            new CustomerRideRequestedHandler(customerListRepository));
    }

    public string AddCab()
    {
        var cabName = "Evan's Cab";
        _applicationHandler.Handle(new AddCabCommand(cabName, 46.2382, 63.1311));

        return "Added Evan's Cab to fleet";
    }

    public string RemoveCab()
    {
        try
        {
            _applicationHandler.Handle(new RemoveCabCommand(1));
            return "Requested cab removed from fleet";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string CustomerCabCall(string? customerName, string? startLocation, string? destinationLane)
    {
        _applicationHandler.Handle(new CustomerCabRequested(customerName, startLocation, destinationLane));
        return $"Received customer ride request from {customerName}";
    }

    public List<string> CustomerCancelledCabRide()
    {
        try
        {
            var menuObj = _menuRepository.GetById(1);
            var hasOption = menuObj.MenuOptions().Contains(6);
            if (!hasOption)
            {
                throw new SystemException("This is not a valid option.");
            }

            // _cabServiceHandler.CancelPickup();
            _applicationHandler.Handle(new CustomerCancelledCab());
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
            var menuObj = _menuRepository.GetById(1);
            var hasOption = menuObj.MenuOptions().Contains(3);
            if (!hasOption)
            {
                throw new SystemException("This is not a valid option.");
            }

            // var response = _cabServiceHandler.SendCabRequest();
            _applicationHandler.Handle(new CustomerRideRequested());

            // TODO: Fix this - it may have to do a read, which kinda sucks
            var customerList = _customerListRepository.GetById(1);
            // TODO: fix this, should not be hardcodedZ
            var customer = customerList.Customers.Last(x => x.Status == CustomerStatus.WaitingPickup);
            return 
            [
                $"Evan's Cab picked up {customer.Name} at {customer.StartLocation}.",
                "Cab assigned to customer."
            ];;
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
            var menuObj = _menuRepository.GetById(1);
            var hasOption = menuObj.MenuOptions().Contains(4);
            if (!hasOption)
            {
                throw new SystemException("This is not a valid option.");
            }

            _applicationHandler.Handle(new CustomerPickedUp());
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
            var menuObj = _menuRepository.GetById(1);
            var hasOption = menuObj.MenuOptions().Contains(5);
            if (!hasOption)
            {
                throw new SystemException("This is not a valid option.");
            }

            _applicationHandler.Handle(new CustomerDelivered());
            var customerList = _customerListRepository.GetById(1);
            // TODO: fix this, should not be hardcodedZ
            var customer = customerList.Customers.Last(x => x.Status == CustomerStatus.Delivered);
            return
            [
                $"Evan's Cab dropped off {customer.Name} at {customer.EndLocation}."
            ];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }
}