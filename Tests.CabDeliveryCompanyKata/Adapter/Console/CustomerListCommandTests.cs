using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class CustomerListCommandsTests
{
    private readonly CabContext _cabContext;
    private CustomerListRepository _customerListRepository;
    private CustomerCabRequestedHandler _customerCabRequestedHandler;
    private readonly CustomerDeliveredHandler _customerDeliveredHandler;
    private readonly CustomerPickedUpHandler _customerPickedUpHandler;
    private readonly CustomerRideRequestedHandler _customerRideRequestedHandler;
    private CustomerCancelledCabHandler _customerCancelledCabHandler;

    public CustomerListCommandsTests()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");
        _cabContext = new CabContext(dbContextOptionsBuilder.Options);
        _cabContext.Database.Migrate();
        _cabContext.ChangeTracker.Clear();
        _customerListRepository = new CustomerListRepository(_cabContext);
        _customerCabRequestedHandler = new CustomerCabRequestedHandler(_customerListRepository);
        _customerDeliveredHandler = new CustomerDeliveredHandler(_customerListRepository);
        _customerPickedUpHandler = new CustomerPickedUpHandler(_customerListRepository);
        _customerRideRequestedHandler = new CustomerRideRequestedHandler(_customerListRepository);
        _customerCancelledCabHandler = new CustomerCancelledCabHandler(_customerListRepository);
    }
    [Fact]
    public void CustomerCanRequestCab()
    {
        var handle = _customerCabRequestedHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));
        Assert.Equal(1, handle);
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
        _customerCabRequestedHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerCabRequestedHandler.Handle(new CustomerCabRequested("Lisa", "1 Fulton Drive", "2 Destination Lane"));
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
        _customerCabRequestedHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destination Lane"));      Assert.Equal(1, _cabContext.CustomerList.Count());
        _customerRideRequestedHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
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
        _customerCabRequestedHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _customerRideRequestedHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
        _customerCabRequestedHandler.Handle(new CustomerCabRequested("Lisa", "1 Fulton Drive", "2 Destionation Lane"));
        _customerRideRequestedHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});      
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
        _customerCabRequestedHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _customerRideRequestedHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});
        _customerPickedUpHandler.Handle(new CustomerPickedUp());
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
        _customerCabRequestedHandler.Handle(new CustomerCabRequested("Dan", "1 Fulton Drive", "2 Destionation Lane"));
        _customerRideRequestedHandler.Handle(new CustomerRideRequested() { CustomerListId = 1});  
        _customerPickedUpHandler.Handle(new CustomerPickedUp());
        _customerDeliveredHandler.Handle(new CustomerDelivered());
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
        _customerCabRequestedHandler.Handle(
            new CustomerCabRequested(
                "Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerCancelledCabHandler.Handle(new CustomerCancelledCab());
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