using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Domain;
using Tests.CabDeliveryCompanyKata;

namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class DispatchController
{
    private readonly CustomerListRepository _customerListRepository;
    private readonly FleetRepository _fleetRepository;
    private readonly MenuRepository _menuRepository;
    private CustomerCabRequestedHandler _customerCabRequestedHandler;
    private CustomerRideRequestedHandler _customerRideRequestedHandler;
    private CustomerCancelledCabHandler _customerCancelledCabHandler;
    private CustomerDeliveredHandler _customerDeliveredHandler;
    private CustomerPickedUpHandler _customerPickedUpHandler;
    private AddCabCommandHandler _addCabCommandHandler;
    private RemoveCabCommandHandler _removeCabCommandHandler;

    public DispatchController(CustomerListRepository customerListRepository, FleetRepository fleetRepository, MenuRepository menuRepository)
    {
        _customerListRepository = customerListRepository;
        _fleetRepository = fleetRepository;
        _menuRepository = menuRepository;
        _customerCabRequestedHandler = new CustomerCabRequestedHandler(_customerListRepository);
        _customerCancelledCabHandler = new CustomerCancelledCabHandler(_customerListRepository);
        _customerDeliveredHandler = new CustomerDeliveredHandler(_customerListRepository);
        _customerPickedUpHandler = new CustomerPickedUpHandler(_customerListRepository);
        _customerRideRequestedHandler = new CustomerRideRequestedHandler(_customerListRepository);
        _addCabCommandHandler = new AddCabCommandHandler(_fleetRepository);
        _removeCabCommandHandler = new RemoveCabCommandHandler(_fleetRepository);
    }

    public string AddCab()
    {
        var cabName = "Evan's Cab";
        _addCabCommandHandler.Handle(new AddCabCommand(cabName, 46.2382, 63.1311));

        return "Added Evan's Cab to fleet";
    }

    public string RemoveCab()
    {
        try
        {
            _removeCabCommandHandler.Handle(new RemoveCabCommand(1));
            return "Requested cab removed from fleet";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string CustomerCabCall(string? customerName, string? startLocation, string? destinationLane)
    {
        _customerCabRequestedHandler.Handle(new CustomerCabRequested(customerName, startLocation, destinationLane));
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
            var menuObj = _menuRepository.GetById(1);
            var hasOption = menuObj.MenuOptions().Contains(3);
            if (!hasOption)
            {
                throw new SystemException("This is not a valid option.");
            }

            // var response = _cabServiceHandler.SendCabRequest();
            _customerRideRequestedHandler.Handle(new CustomerRideRequested());

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
            var menuObj = _menuRepository.GetById(1);
            var hasOption = menuObj.MenuOptions().Contains(5);
            if (!hasOption)
            {
                throw new SystemException("This is not a valid option.");
            }

            _customerDeliveredHandler.Handle(new CustomerDelivered());
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