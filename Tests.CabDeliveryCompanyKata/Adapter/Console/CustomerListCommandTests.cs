using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
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
        _customerListRepository = new CustomerListRepository(_cabContext);
        _fleetRepository = new FleetRepository(_cabContext);
        _applicationHandler = new ApplicationHandler(new FleetRepository(_cabContext), new CustomerListRepository(_cabContext));
    }
    [Fact]
    public void CustomerCanRequestCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));
        Assert.Equal("Dan", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers.FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.CustomerCallInProgress, _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers.FirstOrDefault()!.Status);
        
    }
    [Fact]
    public void TwoCustomersCanRequestCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _applicationHandler.Handle(new CustomerCabRequested("Lisa", "1 Fulton Drive", "2 Destination Lane"));
        Assert.Equal(1, _cabContext.CustomerList.Count());
        Assert.Equal(2, _cabContext.CustomerList
            .Include(customerList => customerList.Customers!)
            .FirstOrDefault()!.Customers!.Count);
        Assert.Equal("Dan", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers.FirstOrDefault()!.Name);
        Assert.Equal("Lisa", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers.Skip(1).FirstOrDefault()!.Name);
    }
    [Fact]
    public void CustomerCanRequestCabAndBeSentCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));      Assert.Equal(1, _cabContext.CustomerList.Count());
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
        Assert.Equal(1, _cabContext.CustomerList.Count());
        Assert.Equal("Dan", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers.FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.WaitingPickup, _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers.FirstOrDefault()!.Status);
    }
    [Fact]
    public void TwoCustomersCanRequestCabAndBeSentCab()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
        _applicationHandler.Handle(new CustomerCabRequested("Lisa", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
        Assert.Equal(2, _cabContext.CustomerList
            .Include(customerList => customerList.Customers!)
            .FirstOrDefault()!.Customers!.Count);
        Assert.Equal("Lisa", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .Skip(1)
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.WaitingPickup, _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .Skip(1)
            .FirstOrDefault()!.Status);
    }
    [Fact]
    public void CustomerCanBePickedUp()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});
        _applicationHandler.Handle(new CustomerPickedUp());
        Assert.Equal(1, _cabContext.CustomerList
            .Include(customerList => customerList.Customers!)
            .FirstOrDefault()!.Customers!.Count);
        Assert.Equal("Dan", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.Enroute, _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .FirstOrDefault()!.Status);
    }
    [Fact]
    public void CustomerHasBeenDelivered()
    {
        _applicationHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _applicationHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});  
        _applicationHandler.Handle(new CustomerPickedUp());
        _applicationHandler.Handle(new CustomerDelivered());
        Assert.Equal(1, _cabContext.CustomerList
            .Include(customerList => customerList.Customers!)
            .FirstOrDefault()!.Customers!.Count);
        Assert.Equal("Dan", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.Delivered, _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .FirstOrDefault()!.Status);
    }
    [Fact]
    public void CustomerHasCancelled()
    {
        _applicationHandler.Handle(
            new CustomerCabRequested(
                "Dan", "1 Fulton Drive", "2 Destination Lane"));
        _applicationHandler.Handle(new CustomerCancelledCab());
        Assert.Equal(1, _cabContext.CustomerList
            .Include(customerList => customerList.Customers!)
            .FirstOrDefault()!.Customers!.Count);
        Assert.Equal("Dan", _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .FirstOrDefault()!.Name);
        Assert.Equal(CustomerStatus.CancelledCall, _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()
            !.Customers
            .FirstOrDefault()!.Status);
    }
}