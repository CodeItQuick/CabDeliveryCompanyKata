using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.@out;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class MenuControllerTest
{
    [Fact]
    public void CanDisplayStartMenu()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var menuService = new MenuService(dispatcherCoordinator);
        var menuController = new MenuController(menuService);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equal("Please choose a selection from the list: ", displayMenu.First());
        Assert.Equal("0. Exit", displayMenu.Skip(1).First());
        Assert.Equal("1. (Incoming Radio) Add New Cab Driver", displayMenu.Skip(2).First());
        Assert.Equal("2. (Incoming Radio) Remove Cab Driver", displayMenu.Skip(3).First());
        Assert.Equal("7. (Incoming Call) Customer Request Ride", displayMenu.Skip(4).First());
    }
    [Fact]
    public void CanDisplayMultipleOptionsMenu()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var menuService = new MenuService(dispatcherCoordinator);
        var menuController = new MenuController(menuService);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equal("Please choose a selection from the list: ", displayMenu.First());
        Assert.Equal("0. Exit", displayMenu.Skip(1).First());
        Assert.Equal("1. (Incoming Radio) Add New Cab Driver", displayMenu.Skip(2).First());
        Assert.Equal("2. (Incoming Radio) Remove Cab Driver", displayMenu.Skip(3).First());
        Assert.Equal("3. (Outgoing Radio) Send Cab Driver Ride Request", displayMenu.Skip(4).First());
        Assert.Equal("6. (Incoming Call) Cancel Cab Driver Fare", displayMenu.Skip(5).First());
        Assert.Equal("7. (Incoming Call) Customer Request Ride", displayMenu.Skip(6).First());
    }
    [Fact]
    public void CanDisplayFullOptionsMenu()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var menuService = new MenuService(dispatcherCoordinator);
        var menuController = new MenuController(menuService);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.AddCab(new Cab("Evan", 20));
        dispatcherCoordinator.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatcherCoordinator.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatcherCoordinator.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatcherCoordinator.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatcherCoordinator.RideRequest();
        dispatcherCoordinator.RideRequest();
        dispatcherCoordinator.RideRequest();
        dispatcherCoordinator.PickupCustomer();
        dispatcherCoordinator.PickupCustomer();
        dispatcherCoordinator.DropOffCustomer();

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equal("Please choose a selection from the list: ", displayMenu.First());
        Assert.Equal("0. Exit", displayMenu.Skip(1).First());
        Assert.Equal("1. (Incoming Radio) Add New Cab Driver", displayMenu.Skip(2).First());
        Assert.Equal("2. (Incoming Radio) Remove Cab Driver", displayMenu.Skip(3).First());
        Assert.Equal("3. (Outgoing Radio) Send Cab Driver Ride Request", displayMenu.Skip(4).First());
        Assert.Equal("4. (Incoming Radio) Cab Notifies Passenger Picked Up", displayMenu.Skip(5).First());
        Assert.Equal("5. (Incoming Radio) Cab Notifies Passenger Dropped Off", displayMenu.Skip(6).First());
        Assert.Equal("6. (Incoming Call) Cancel Cab Driver Fare", displayMenu.Skip(7).First());
        Assert.Equal("7. (Incoming Call) Customer Request Ride", displayMenu.Skip(8).First());
    }
}