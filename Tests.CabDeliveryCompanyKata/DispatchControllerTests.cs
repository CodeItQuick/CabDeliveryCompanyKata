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
        dispatchController.CustomerCabCall(["Dan"]);
        dispatchController.SendCabRequest();
        dispatchController.CabNotifiesPickedUp();
        
        var result = dispatchController.RemoveCab();
        
        Assert.Equal("Cab cannot be removed until passenger dropped off.", result);
    }
}