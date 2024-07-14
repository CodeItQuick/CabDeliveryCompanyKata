using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;

namespace Tests.CabDeliveryCompanyKata;

public class DispatchControllerTests
{
    [Fact]
    public void CanAddCabsToTheFleet()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);

        var addCabMessage = dispatchController.AddCab();

        Assert.Equal("Added Evan's Cab to fleet", addCabMessage);
    }
    [Fact]
    public void CanRemoveCabsFromTheFleet()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
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
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);

        var result = dispatchController.RemoveCab();
        
        Assert.Equal("Cab cannot be removed until passenger dropped off.", result);
    }
    [Fact]
    public void CannotRemoveCabsWithPassengerInIt()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
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
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
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
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();

        var customerCabCall = dispatchController.CustomerCabCall();
        
        Assert.Equal("Received customer ride request from Emma", customerCabCall);
    }
    [Fact]
    public void TwoCustomersCanCallInCab()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var customerCabCall = dispatchController.CustomerCabCall();
        
        Assert.Equal("Received customer ride request from Lisa", customerCabCall);
    }
    [Fact]
    public void FirstCustomerCallInCancelsSecondCustomerCanCallInCab()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.CustomerCancelledCabRide();

        var customerCabCall = dispatchController.CustomerCabCall();
        
        Assert.Equal("Received customer ride request from Lisa", customerCabCall);
    }
    [Fact]
    public void CannotPickupUnlessCustomersWaiting()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        
        var customerCabCall = dispatchController.CustomerCancelledCabRide();
        
        Assert.Equal("No customers are waiting for pickup. Cannot cancel cab.", customerCabCall.First());
    }
    [Fact]
    public void CanCancelPickup()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        
        var customerCabCall = dispatchController.CustomerCancelledCabRide();
        
        Assert.Equal("Customer cancelled cab ride successfully.", customerCabCall.First());
    }
    [Fact]
    public void CanCancelPickupAtAnyTime()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();
        
        var customerCabCall = dispatchController.CustomerCancelledCabRide();
        
        Assert.Equal("Customer cancelled cab ride successfully.", customerCabCall.First());
    }
    [Fact]
    public void CabCanDriveToCustomerAfterCabRequest()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var sendCabRequest = dispatchController.SendCabRequest();

        Assert.Equal("Evan's Cab picked up Emma at 1 Fulton Drive.", sendCabRequest.First());
        Assert.Equal("Cab assigned to customer.", sendCabRequest.Skip(1).First());
    }
    [Fact]
    public void CannotSendCabRequestUntilCustomerCallsIn()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();

        var sendCabRequest = dispatchController.SendCabRequest();

        Assert.Equal("There are currently no customer's waiting for cabs.", sendCabRequest.First());
    }
    [Fact]
    public void CannotSendCabRequestUntilCabsAreInFleet()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);

        var sendCabRequest = dispatchController.SendCabRequest();

        Assert.Equal("There are currently no cabs in the fleet.", sendCabRequest.First());
    }
    [Fact]
    public void CabCanPickupCustomer()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();
        dispatchController.SendCabRequest();

        var sendCabRequest = dispatchController.CabNotifiesPickedUp();

        Assert.Equal("Notified dispatcher of pickup", sendCabRequest);
    }
    [Fact]
    public void CabCannotPickupCustomerIfNoCabsInFleet()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);

        var sendCabRequest = dispatchController.CabNotifiesPickedUp();

        Assert.Equal("There are currently no cabs in the fleet.", sendCabRequest);
    }
    [Fact]
    public void CabCannotPickupCustomerIfCustomerNotWaitingPickup()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var sendCabRequest = dispatchController.CabNotifiesPickedUp();

        Assert.Equal("There are currently no customer's assigned to cabs.", sendCabRequest);
    }
    [Fact]
    public void CabCanDropOffCustomer()
    {
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
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
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
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
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
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
        var radioFleet = new RadioFleet();
        var dispatchController = new DispatchController(radioFleet);
        dispatchController.AddCab();
        dispatchController.CustomerCabCall();

        var droppedOff = dispatchController.CabNotifiesDroppedOff();

        Assert.Equal("There are currently no customer's assigned to cabs.", droppedOff.First());
    }
}