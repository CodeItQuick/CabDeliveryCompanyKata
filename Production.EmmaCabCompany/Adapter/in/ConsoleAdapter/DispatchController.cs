using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Patrons;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class DispatchController
{
    private readonly PatronRepository _patronRepository;
    private ApplicationHandler _applicationHandler;

    public DispatchController(CabContext cabContext)
    {
        _patronRepository = new PatronRepository(cabContext);
        _applicationHandler = new ApplicationHandler(cabContext);
    }

    public string AddCab(int? userId)
    {
        var cabName = "Evan's Cab";
        _applicationHandler.Handle(new AddCabCommand(cabName, 46.2382, 63.1311, userId));

        return "Added Evan's Cab to fleet";
    }

    public string RemoveCab(int? userId)
    {
        try
        {
            _applicationHandler.Handle(new RemoveCabCommand(userId));
            return "Requested cab removed from fleet";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string CustomerCabCall(string? customerName, string? startLocation, string? destinationLane, int? userId)
    {
        _applicationHandler.Handle(new CustomerCabRequested(customerName, startLocation, destinationLane, userId ?? 0));
        return $"Received customer ride request from {customerName}";
    }

    public List<string> CustomerCancelledCabRide(int? userId)
    {
        try
        {
            // var menuObj = _menuRepository.GetById(new Menu() { Id = userId });
            // var hasOption = menuObj.MenuOptions().Contains("6");
            // if (!hasOption)
            // {
            //     throw new SystemException("This is not a valid option.");
            // }
            //
            // // _cabServiceHandler.CancelPickup();
            // _applicationHandler.Handle(new CustomerCancelledCab());
            return ["Customer cancelled cab ride successfully."];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }

    public List<string> SendCabRequest(int? userId)
    {
        try
        {
            // var menuObj = _menuRepository.GetById(new Menu() { Id = userId});
            // var hasOption = menuObj.MenuOptions().Contains("3");
            // if (!hasOption)
            // {
            //     throw new SystemException("This is not a valid option.");
            // }
            //
            // // var response = _cabServiceHandler.SendCabRequest();
            // _applicationHandler.Handle(new CustomerRideRequested());
            //
            // // TODO: Fix this - it may have to do a read, which kinda sucks
            // var customerList = _patronRepository.GetById(new PatronList() { Id = userId });
            // // TODO: fix this, should not be hardcodedZ
            // var customer = customerList.Customers.Last(x => x.Status == PatronStatus.WaitingPickup);
            return 
            [
                $"Evan's Cab picked up Customer at Start Location.",
                "Cab assigned to customer."
            ];;
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }

    public string CabNotifiesPickedUp(int? userId)
    {
        try
        {
            // var menuObj = _menuRepository.GetById(new Menu() { Id = userId});
            // var hasOption = menuObj.MenuOptions().Contains("4");
            // if (!hasOption)
            // {
            //     throw new SystemException("This is not a valid option.");
            // }
            //
            // _applicationHandler.Handle(new CustomerPickedUp());
            return "Notified dispatcher of pickup";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public List<string> CabNotifiesDroppedOff(int? userId)
    {
        try
        {
            // var menuObj = _menuRepository.GetById(new Menu() { Id = userId });
            // var hasOption = menuObj.MenuOptions().Contains("5");
            // if (!hasOption)
            // {
            //     throw new SystemException("This is not a valid option.");
            // }
            //
            // _applicationHandler.Handle(new CustomerDelivered());
            // var customerList = _patronRepository.GetById(new PatronList() { Id = userId });
            // // TODO: fix this, should not be hardcodedZ
            // var customer = customerList.Customers.Last(x => x.Status == PatronStatus.Delivered);
            return
            [
                $"Evan's Cab dropped off Patron at End Location."
            ];
        }
        catch (Exception ex)
        {
            return [ex.Message];
        }
    }

    public string RegisterNewUser()
    {
        _applicationHandler.Handle(new NewUserRegistered());
        return "A new user has been registered";
    }

    public string LoginUser(int? userId)
    {
        _applicationHandler.Handle(new UserLogin() { Id = userId });
        return "An existing user has logged in successfully";
    }
}