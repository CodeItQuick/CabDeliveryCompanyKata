using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata;

public class MenuServiceTests
{
    [Fact]
    public void CanDisplayStartMenu()
    {
        var menuController = new MenuService(new DispatcherCoordinator(), new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"customer_list_{Guid.NewGuid()}.csv")));

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplaySecondMenuWithCabRequest()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuController = new MenuService(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 3, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayThirdMenuWithPickUpRequest()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuController = new MenuService(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.RideRequest();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 4, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayDropOffUpdate()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuController = new MenuService(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.RideRequest();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.PickupCustomer();
        ExportPersistence(dispatcherCoordinator, cabFileRepository);

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 5, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayCancelCab()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuController = new MenuService(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        
        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 3, 6, 7 }, displayMenu);
    }
    [Fact]
    public void CabAndCustomerCallInCanDisplayFullMenu()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuController = new MenuService(dispatcherCoordinator, cabFileRepository);
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
        
        Assert.Equivalent(new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7 }, displayMenu);
    }
    [Fact]
    public void KnowsIfOptionIsInvalid()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var menuController = new MenuService(dispatcherCoordinator, new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv")));

        var displayMenu = menuController.IsValidMenuOption(3);
        
        Assert.False(displayMenu);
    }
    [Fact]
    public void KnowsIfOptionIsValid()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(new FakeFileReadWriter($"customer_list_{Guid.NewGuid()}.csv", $"cab_list_{Guid.NewGuid()}.csv"));
        var menuController = new MenuService(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.AddCab(new Cab("Evan", 20, 46.2382, 63.1311));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        dispatcherCoordinator.CustomerCabCall(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
        ExportPersistence(dispatcherCoordinator, cabFileRepository);
        
        var displayMenu = menuController.IsValidMenuOption(3);
        
        Assert.True(displayMenu);
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