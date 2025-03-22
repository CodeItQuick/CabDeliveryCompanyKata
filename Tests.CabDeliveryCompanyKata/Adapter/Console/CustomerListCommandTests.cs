using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class CustomerListCommandsTests
{
    private readonly CabContext _cabContext;
    private CustomerListRepository _customerListRepository;
    private FleetRepository _fleetRepository;
    private ApplicationHandler _applicationHandler;

    public CustomerListCommandsTests()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");
        _cabContext = new CabContext(dbContextOptionsBuilder.Options);
        _cabContext.Database.Migrate();
        _cabContext.ChangeTracker.Clear();
        EnsureFleetExistsForSingleUser();
        EnsureMenuExistsForSingleUser();
        _customerListRepository = new CustomerListRepository(_cabContext);
        _fleetRepository = new FleetRepository(_cabContext);
        _applicationHandler = new ApplicationHandler(_cabContext);
    }
    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Menu() { Id = 1, Customers = new List<PatronDto>()});
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.FleetCoordinator.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.FleetCoordinator.Add(new FleetCoordinator() { Id = 1, Patrons = new List<PatronDto>() });
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }
    [Fact]
    public void CustomerCanRequestCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));
        Assert.Equal("Dan", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons.FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.CustomerCallInProgress, _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons.FirstOrDefault()!.Status);
        
    }
    [Fact]
    public void TwoCustomersCanRequestCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _applicationHandler.Handle(new CustomerCabRequested("Lisa", "1 Fulton Drive", "2 Destination Lane"));
        Assert.Equal(1, _cabContext.FleetCoordinator.Count());
        Assert.Equal(2, _cabContext.FleetCoordinator
            .Include(customerList => customerList.Patrons!)
            .FirstOrDefault()!.Patrons!.Count);
        Assert.Equal("Dan", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons.FirstOrDefault()!.Name);
        Assert.Equal("Lisa", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons.Skip(1).FirstOrDefault()!.Name);
    }
    [Fact]
    public void CustomerCanRequestCabAndBeSentCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));      Assert.Equal(1, _cabContext.FleetCoordinator.Count());
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
        Assert.Equal(1, _cabContext.FleetCoordinator.Count());
        Assert.Equal("Dan", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons.FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.WaitingPickup, _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons.FirstOrDefault()!.Status);
    }
    [Fact]
    public void TwoCustomersCanRequestCabAndBeSentCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
        _applicationHandler.Handle(new CustomerCabRequested("Lisa", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
        Assert.Equal(2, _cabContext.FleetCoordinator
            .Include(customerList => customerList.Patrons!)
            .FirstOrDefault()!.Patrons!.Count);
        Assert.Equal("Lisa", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .Skip(1)
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.WaitingPickup, _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .Skip(1)
            .FirstOrDefault()!.Status);
    }
    [Fact]
    public void CustomerCanBePickedUp()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});
        _applicationHandler.Handle(new CustomerPickedUp());
        Assert.Equal(1, _cabContext.FleetCoordinator
            .Include(customerList => customerList.Patrons!)
            .FirstOrDefault()!.Patrons!.Count);
        Assert.Equal("Dan", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.Enroute, _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .FirstOrDefault()!.Status);
    }
    [Fact]
    public void CustomerHasBeenDelivered()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});  
        _applicationHandler.Handle(new CustomerPickedUp());
        _applicationHandler.Handle(new CustomerDelivered());
        Assert.Equal(1, _cabContext.FleetCoordinator
            .Include(customerList => customerList.Patrons!)
            .FirstOrDefault()!.Patrons!.Count);
        Assert.Equal("Dan", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.Delivered, _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .FirstOrDefault()!.Status);
    }
    [Fact]
    public void CustomerHasCancelled()
    {
        _applicationHandler.Handle(
            new CustomerCabRequested(
                "Dan", "1 Fulton Drive", "2 Destination Lane"));
        _applicationHandler.Handle(new CustomerCancelledCab());
        Assert.Equal(1, _cabContext.FleetCoordinator
            .Include(customerList => customerList.Patrons!)
            .FirstOrDefault()!.Patrons!.Count);
        Assert.Equal("Dan", _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.CancelledCall, _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault()
            !.Patrons
            .FirstOrDefault()!.Status);
    }
}