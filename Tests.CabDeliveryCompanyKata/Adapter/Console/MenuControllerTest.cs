using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;
using Production.EmmaCabCompany.Adapter.@out;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class MenuControllerTest
{
    [Fact]
    public void CanDisplayStartMenu()
    {
        var dbContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}.db")
            .Options;
        using var cabContext = new CabContext(dbContextOptions);
        var menuController = new MenuController(new MenuRepository(cabContext));

        var displayMenu = menuController.DisplayMenu();
        
        Assert.Equal("Please choose a selection from the list: ", displayMenu.First());
        Assert.Equal("0. Exit", displayMenu.Skip(1).First());
        Assert.Equal("1. (Incoming Radio) Add New Cab Driver", displayMenu.Skip(2).First());
    }
    [Fact]
    public void CanMutateDisplayMenu()
    {
        var dispatcherCoordinator = new DispatcherCoordinator();
        var dbContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}.db")
            .Options;
        using var cabContext = new CabContext(dbContextOptions);
        var menuController = new MenuController(new MenuRepository(cabContext));

        var dispatchController = new DispatchController(new CustomerListRepository(cabContext), new FleetRepository(cabContext), new MenuRepository(cabContext));

        dispatchController.AddCab();
        
        var displayMenu = menuController.DisplayMenu();
        Assert.Equal("Please choose a selection from the list: ", displayMenu.First());
        Assert.Equal("0. Exit", displayMenu.Skip(1).First());
        Assert.Equal("1. (Incoming Radio) Add New Cab Driver", displayMenu.Skip(2).First());
        Assert.Equal("2. (Incoming Radio) Remove Cab Driver", displayMenu.Skip(3).First());
        Assert.Equal("7. (Incoming Call) Customer Request Ride", displayMenu.Skip(4).First());
    }
}