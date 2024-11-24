using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.Menu;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class MenuCommandTests
{
    private readonly CabContext _cabContext;
    private CustomerListRepository _customerListRepository;
    private CustomerCabRequestedHandler _customerCabRequestedHandler;
    private readonly CustomerDeliveredHandler _customerDeliveredHandler;
    private readonly CustomerEnroutedHandler _customerEnroutedHandler;
    private readonly CustomerPickedUpHandler _customerPickedUpHandler;
    private readonly CustomerRideRequestedHandler _customerRideRequestedHandler;
    private CustomerCancelledCabHandler _customerCancelledCabHandler;
    private MenuRepository _menuRepository;
    private MenuRequestedHandler _menuRequestedHandler;

    public MenuCommandTests()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");
        _cabContext = new CabContext(dbContextOptionsBuilder.Options);
        _cabContext.Database.Migrate();
        _menuRepository = new MenuRepository(_cabContext);
        _menuRequestedHandler = new MenuRequestedHandler(_menuRepository);
    }

    [Fact]
    public void MenuDisplaysDefaultOptions()
    {
        var handle = _menuRequestedHandler.Handle(new MenuRequested(1));
        Assert.NotNull(handle);
        Assert.Contains(0, handle.MenuOptions);
        Assert.Contains(1, handle.MenuOptions);
    }

    [Fact]
    public void MenuDisplaysOptionsThreeAndSixWhenCustomerCallInProgress()
    {
        _cabContext.Customers.Add(
            new CustomerDto()
            {
                Name = "Dan", StartLocation = "1 Fulton Drive", EndLocation = "2 Destination Lane",
                Status = CustomerStatus.CustomerCallInProgress, MenuId = 1, CustomerId = 1
            });
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();

        var handle = _menuRequestedHandler.Handle(new MenuRequested(1));
        Assert.NotNull(handle);
        Assert.Contains(0, handle.MenuOptions);
        Assert.Contains(1, handle.MenuOptions);
        Assert.Contains(3, handle.MenuOptions);
        Assert.Contains(6, handle.MenuOptions);
    }

    [Fact]
    public void MenuDisplaysOptionsFourWhenCustomerWaitingPickup()
    {
        _cabContext.Customers.Add(
            new CustomerDto()
            {
                Name = "Dan", StartLocation = "1 Fulton Drive", EndLocation = "2 Destination Lane",
                Status = CustomerStatus.WaitingPickup, MenuId = 1, CustomerId = 1
            });

        _cabContext.SaveChanges();

        var handle = _menuRequestedHandler.Handle(new MenuRequested(1));
        Assert.NotNull(handle);
        Assert.Contains(0, handle.MenuOptions);
        Assert.Contains(1, handle.MenuOptions);
        Assert.Contains(4, handle.MenuOptions);
    }

    [Fact]
    public void MenuDisplaysOptionsFiveWhenCustomerEnroute()
    {
        _cabContext.Customers.Add(
            new CustomerDto()
            {
                Name = "Dan", StartLocation = "1 Fulton Drive", EndLocation = "2 Destination Lane",
                Status = CustomerStatus.Enroute, MenuId = 1, CustomerId = 1
            });
        _cabContext.SaveChanges();

        var handle = _menuRequestedHandler.Handle(new MenuRequested(1));
        Assert.NotNull(handle);
        Assert.Contains(0, handle.MenuOptions);
        Assert.Contains(1, handle.MenuOptions);
        Assert.Contains(5, handle.MenuOptions);
    }
}