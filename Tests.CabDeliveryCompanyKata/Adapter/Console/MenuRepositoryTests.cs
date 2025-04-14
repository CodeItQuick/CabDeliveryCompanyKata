using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Application.Menu;
using Production.EmmaCabCompany.Domain.CustomerList;
using Production.EmmaCabCompany.Domain.Menu;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class MenuCommandTests
{
    // private readonly CabContext _cabContext;
    // private MenuRepository _menuRepository;
    // private MenuRequestedHandler _menuRequestedHandler;
    //
    // public MenuCommandTests()
    // {
    //     var dbContextOptionsBuilder = new DbContextOptionsBuilder<CabContext>()
    //         .UseInMemoryDatabase($"Data Source={Guid.NewGuid()}");
    //     _cabContext = new CabContext(dbContextOptionsBuilder.Options);
    // }
    //
    // [Fact]
    // public void MenuDisplaysDefaultOptions()
    // {
    //     _menuRepository = new MenuRepository(_cabContext);
    //     _menuRequestedHandler = new MenuRequestedHandler(_menuRepository);
    //     
    //     var handle = _menuRequestedHandler
    //         .Handle(new MenuRequested(1));
    //     
    //     Assert.NotNull(handle);
    //     Assert.Contains("0", handle.MenuOptions);
    //     Assert.Contains("8", handle.MenuOptions);
    //     Assert.Contains("9", handle.MenuOptions);
    // }
    //
    // [Fact]
    // public void MenuDisplaysOptionsThreeAndSixWhenCustomerCallInProgress()
    // {
    //     var patronDto = new PatronDto()
    //     {
    //         Name = "Dan", StartLocation = "1 Fulton Drive", EndLocation = "2 Destination Lane",
    //         Status = PatronStatus.CustomerCallInProgress, MenuId = 1, 
    //     };
    //     _cabContext.Patrons.Add(patronDto);
    //     _cabContext.Menues.Add(new Menu() { Customer = new List<FleetCoordinator>()
    //             {
    //                 new FleetCoordinator() { Patrons = [patronDto]}
    //             }});
    //     _cabContext.SaveChanges();
    //     _cabContext.ChangeTracker.Clear();
    //     _menuRepository = new MenuRepository(_cabContext);
    //     _menuRequestedHandler = new MenuRequestedHandler(_menuRepository);
    //
    //     var handle = _menuRequestedHandler.Handle(new MenuRequested(1));
    //     Assert.NotNull(handle);
    //     Assert.Contains("0", handle.MenuOptions);
    //     Assert.Contains("1", handle.MenuOptions);
    //     Assert.Contains("3", handle.MenuOptions);
    //     Assert.Contains("6", handle.MenuOptions);
    // }
    //
    // [Fact]
    // public void MenuDisplaysOptionsFourWhenCustomerWaitingPickup()
    // {
    //     var patronDto = new PatronDto()
    //     {
    //         Name = "Dan", StartLocation = "1 Fulton Drive", EndLocation = "2 Destination Lane",
    //         Status = PatronStatus.WaitingPickup, MenuId = 1, 
    //     };
    //     _cabContext.Patrons.Add(
    //         patronDto);
    //     _cabContext.Menues.Add(new Menu() { Customer = new List<FleetCoordinator>()
    //     {
    //         new() { Patrons = [patronDto]}
    //     }});
    //     _menuRepository = new MenuRepository(_cabContext);
    //     _menuRequestedHandler = new MenuRequestedHandler(_menuRepository);
    //
    //     _cabContext.SaveChanges();
    //
    //     var handle = _menuRequestedHandler.Handle(new MenuRequested(1));
    //     Assert.NotNull(handle);
    //     Assert.Contains("0", handle.MenuOptions);
    //     Assert.Contains("1", handle.MenuOptions);
    //     Assert.Contains("4", handle.MenuOptions);
    // }
    //
    // [Fact]
    // public void MenuDisplaysOptionsFiveWhenCustomerEnroute()
    // {
    //     var patronDto = new PatronDto()
    //     {
    //         Name = "Dan", StartLocation = "1 Fulton Drive", EndLocation = "2 Destination Lane",
    //         Status = PatronStatus.Enroute, MenuId = 1, 
    //     };
    //     _cabContext.Patrons.Add(
    //         patronDto);
    //     _cabContext.Menues.Add(new Menu()
    //     {
    //         Customer = new List<FleetCoordinator>()
    //         {
    //             new() { Patrons = [patronDto]}
    //         }});
    //     _cabContext.SaveChanges();
    //     _menuRepository = new MenuRepository(_cabContext);
    //     _menuRequestedHandler = new MenuRequestedHandler(_menuRepository);
    //
    //     var handle = _menuRequestedHandler.Handle(new MenuRequested(1));
    //     Assert.NotNull(handle);
    //     Assert.Contains("0", handle.MenuOptions);
    //     Assert.Contains("1", handle.MenuOptions);
    //     Assert.Contains("5", handle.MenuOptions);
    // }
}