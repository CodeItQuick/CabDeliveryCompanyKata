using Production.EmmaCabCompany.Adapter.@out;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;

namespace Tests.CabDeliveryCompanyKata;

public class DispatchControllerTests
{
    [Fact]
    public void CanAddCabsToTheFleet()
    {
        var radioFleet = new DispatcherCoordinator();
        var cabService = new CabService(radioFleet, new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv"));
        var dispatchController = new DispatchController
            (cabService, new MenuService(new DispatcherCoordinator()))
            ;

        
        var addCabMessage = dispatchController.AddCab();

        Assert.Equal("Added Evan's Cab to fleet", addCabMessage);
    }
    [Fact]
    public void CanRemoveCabsFromTheFleet()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(new DispatcherCoordinator()));
        dispatchController.AddCab();
        var cabsAvailable = radioFleet.NoCabsInFleet();
        var removeCab = dispatchController.RemoveCab();

        var noCabsInFleet = radioFleet.NoCabsInFleet();

        Assert.False(cabsAvailable);
        Assert.True(noCabsInFleet);
        Assert.Equal("Cab removed from fleet", removeCab);
    }
    [Fact]
    public void CannotRemoveCabsFromEmptyFleet()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(new DispatcherCoordinator()));

        var result = dispatchController.RemoveCab();
        
        Assert.Equal("Cab cannot be removed until passenger dropped off.", result);
    }
    [Fact]
    public void CannotRemoveCabsWithPassengerInIt()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();
        dispatchController.CabNotifiesPickedUp();
        
        var result = dispatchController.RemoveCab();
        
        Assert.Equal("Cab cannot be removed until passenger dropped off.", result);
    }
    [Fact]
    public void CanRemoveCabsWithoutPassengerInside()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(new DispatcherCoordinator()));
        dispatchController.AddCab();
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();
        dispatchController.CabNotifiesPickedUp();
        
        var result = dispatchController.RemoveCab();
        
        Assert.Equal("Cab removed from fleet", result);
    }
    [Fact]
    public void CustomerCanCallInCab()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(new DispatcherCoordinator()));
        dispatchController.AddCab();

        var customerCabCall = dispatchController.CustomerCabCall();
        
        Assert.Equal("Received customer ride request from Emma", customerCabCall);
    }
    [Fact]
    public void TwoCustomersCanCallInCab()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(new DispatcherCoordinator()));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var customerCabCall = dispatchController.CustomerCabCall();
        
        Assert.Equal("Received customer ride request from Lisa", customerCabCall);
    }
    [Fact]
    public void FirstCustomerCallInCancelsSecondCustomerCanCallInCab()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(new DispatcherCoordinator()));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.CustomerCancelledCabRide();

        var customerCabCall = dispatchController.CustomerCabCall();
        
        Assert.Equal("Received customer ride request from Lisa", customerCabCall);
    }
    [Fact]
    public void CannotPickupUnlessCustomersWaiting()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        
        var customerCabCall = dispatchController.CustomerCancelledCabRide();
        
        Assert.Equal("This is not a valid option.", customerCabCall.First());
    }
    [Fact]
    public void CanCancelPickup()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        
        var customerCabCall = dispatchController.CustomerCancelledCabRide();
        
        Assert.Equal("Customer cancelled cab ride successfully.", customerCabCall.First());
    }
    [Fact]
    public void CanCancelPickupAtAnyTime()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();
        
        var customerCabCall = dispatchController.CustomerCancelledCabRide();
        
        Assert.Equal("This is not a valid option.", customerCabCall.First());
    }
    [Fact]
    public void CabCanDriveToCustomerAfterCabRequest()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var sendCabRequest = dispatchController.SendCabRequest();

        Assert.Equal("Evan's Cab picked up Emma at 1 Fulton Drive.", sendCabRequest.First());
        Assert.Equal("Cab assigned to customer.", sendCabRequest.Skip(1).First());
    }
    [Fact]
    public void CannotSendCabRequestUntilCustomerCallsIn()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();

        var sendCabRequest = dispatchController.SendCabRequest();

        Assert.Equal("This is not a valid option.", sendCabRequest.First());
    }
    [Fact]
    public void CannotSendCabRequestUntilCabsAreInFleet()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));

        var sendCabRequest = dispatchController.SendCabRequest();

        Assert.Equal("This is not a valid option.", sendCabRequest.First());
    }
    [Fact]
    public void CabCanPickupCustomer()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();

        var sendCabRequest = dispatchController.CabNotifiesPickedUp();

        Assert.Equal("Notified dispatcher of pickup", sendCabRequest);
    }
    [Fact]
    public void CabCannotPickupCustomerIfNoCabsInFleet()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));

        var sendCabRequest = dispatchController.CabNotifiesPickedUp();

        Assert.Equal("This is not a valid option.", sendCabRequest);
    }
    [Fact]
    public void CabCannotPickupCustomerIfCustomerNotWaitingPickup()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var sendCabRequest = dispatchController.CabNotifiesPickedUp();

        Assert.Equal("This is not a valid option.", sendCabRequest);
    }
    [Fact]
    public void CabCanDropOffCustomer()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();
        dispatchController.CabNotifiesPickedUp();

        var droppedOff = dispatchController.CabNotifiesDroppedOff();

        Assert.Equal("Evan's Cab dropped off Emma at 1 Destination Lane.", droppedOff.First());
    }
    [Fact]
    public void CabCanDropOffOnlyOneCustomerAtATime()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();
        dispatchController.SendCabRequest();
        dispatchController.CabNotifiesPickedUp();
        dispatchController.CabNotifiesPickedUp();

        var droppedOff = dispatchController.CabNotifiesDroppedOff();

        Assert.Equal("Evan's Cab dropped off Emma at 1 Destination Lane.", droppedOff.Single());
    }
    [Fact]
    public void CabCanDropOffTwoCustomers()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();
        dispatchController.SendCabRequest();
        dispatchController.CabNotifiesPickedUp();
        dispatchController.CabNotifiesPickedUp();
        dispatchController.CabNotifiesDroppedOff();

        var droppedOff = dispatchController.CabNotifiesDroppedOff();

        Assert.Equal("Evan's Cab dropped off Lisa at 1 Destination Lane.", droppedOff.Single());
    }
    [Fact]
    public void CabCannotDropOffCustomerIfNotInTransport()
    {
        var radioFleet = new DispatcherCoordinator();
        var dispatchController = new DispatchController(
            new CabService(radioFleet, 
            new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv")
            ), new MenuService(radioFleet));
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var droppedOff = dispatchController.CabNotifiesDroppedOff();

        Assert.Equal(
            "This is not a valid option.", 
            droppedOff.First());
    }

    [Fact]
    public void InvalidOptionSelectedReturnsError()
    {
        var dispatchController = new DispatchController(
            new CabService(
                new DispatcherCoordinator(), 
                new FakeFileReadWriter("customer_filename.csv", "cab_list_filename.csv")
                ), new MenuService(new DispatcherCoordinator()));

        var dispatch = dispatchController.CabNotifiesDroppedOff();
        
        Assert.Equal("This is not a valid option.", dispatch.First());
    }
}