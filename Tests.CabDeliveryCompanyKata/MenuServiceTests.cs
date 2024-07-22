using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class MenuServiceTests
{
    [Fact]
    public void CanDisplayStartMenu()
    {
        var menuController = new MenuService(new DispatcherCoordinator());

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplaySecondMenuWithCabRequest()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma");
        var menuController = new MenuService(dispatcherCoordinator);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 3, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayThirdMenuWithPickUpRequest()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma");
        dispatcherCoordinator.RideRequest();
        var menuController = new MenuService(dispatcherCoordinator);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 4, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayDropOffUpdate()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma");
        dispatcherCoordinator.RideRequest();
        dispatcherCoordinator.PickupCustomer();
        var menuController = new MenuService(dispatcherCoordinator);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 5, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayCancelCab()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma");
        var menuController = new MenuService(dispatcherCoordinator);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 3, 6, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayFullMenu()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma");
        dispatcherCoordinator.CustomerCabCall("Emma");
        dispatcherCoordinator.CustomerCabCall("Emma");
        dispatcherCoordinator.CustomerCabCall("Emma");
        dispatcherCoordinator.RideRequest();
        dispatcherCoordinator.RideRequest();
        dispatcherCoordinator.RideRequest();
        dispatcherCoordinator.PickupCustomer();
        dispatcherCoordinator.PickupCustomer();
        dispatcherCoordinator.DropOffCustomer();
        var menuController = new MenuService(dispatcherCoordinator);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7 }, displayMenu);
    }
    [Fact]
    public void KnowsIfOptionIsInvalid()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var menuController = new MenuService(dispatcherCoordinator);

        var displayMenu = menuController.IsValidMenuOption(3);
        
        Assert.False(displayMenu);
    }
    [Fact]
    public void KnowsIfOptionIsValid()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma");
        var menuController = new MenuService(dispatcherCoordinator);

        var displayMenu = menuController.IsValidMenuOption(3);
        
        Assert.True(displayMenu);
    }
}