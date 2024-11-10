using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.@out;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata;

public class MenuControllerTest
{
    [Fact]
    public void CanDisplayStartMenu()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuService = new MenuService(dispatcherCoordinator, cabFileRepository);
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
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuService = new MenuService(dispatcherCoordinator, cabFileRepository);
        var menuController = new MenuController(menuService);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        
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
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuService = new MenuService(dispatcherCoordinator, cabFileRepository);
        var menuController = new MenuController(menuService);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.RideRequest();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.RideRequest();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.RideRequest();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.PickupCustomer();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.PickupCustomer();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.DropOffCustomer();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        
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
    private void ExportPersistence(DispatcherCoordinator dispatcherCoordinator, CabFileRepository cabFileRepository)
    {
        var exportedCustomerList = dispatcherCoordinator.ExportCustomerList();
        string[] exportedCustomers = exportedCustomerList
            // .Where(x => x.Key.Status != CustomerStatus.Delivered)
            .Select(x => 
                $"{x.Key.Name}," +
                $"{x.Key.StartLocation}," +
                $"{x.Key.EndLocation}," +
                $"{x.Value}," +
                $"{x.Key.PickupLocation.Item1}," +
                $"{x.Key.PickupLocation.Item2}"
            ).ToArray();
        cabFileRepository.WriteCustomerList(exportedCustomers);
        string[] cabList = dispatcherCoordinator.ExportCabList();
        cabFileRepository.WriteCabList(cabList);
    }
}