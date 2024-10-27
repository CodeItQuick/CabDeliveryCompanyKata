using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class CustomerListRepositoryTests
{
    private readonly CabContext _cabContext;
    private CustomerListRepository _customerListRepository;

    public CustomerListRepositoryTests()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");
        _cabContext = new CabContext(dbContextOptionsBuilder.Options);
        _cabContext.Database.Migrate();
        _customerListRepository = new CustomerListRepository(_cabContext);
    }
    [Fact]
    public void CustomerCanRequestCab()
    {
        _customerListRepository.CustomerCabRequest(new Customer("Dan", "1 Fulton Drive", "2 Destination Lane"));
        Assert.Equal(1, _cabContext.CustomerList.Count());
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
        _customerListRepository.CustomerCabRequest(new Customer("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerListRepository.CustomerCabRequest(new Customer("Lisa", "1 Fulton Drive", "2 Destination Lane"));
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
        _customerListRepository.CustomerCabRequest(new Customer("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerListRepository.RideRequest();
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
        _customerListRepository.CustomerCabRequest(new Customer("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerListRepository.RideRequest();
        _customerListRepository.CustomerCabRequest(new Customer("Lisa", "1 Fulton Drive", "2 Destination Lane"));
        _customerListRepository.RideRequest();
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
        _customerListRepository.CustomerCabRequest(new Customer("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerListRepository.RideRequest();
        _customerListRepository.PickupCustomer();
        _customerListRepository.PutCustomerEnroute();
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
        _customerListRepository.CustomerCabRequest(new Customer("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerListRepository.RideRequest();
        _customerListRepository.PickupCustomer();
        _customerListRepository.PutCustomerEnroute();
        _customerListRepository.PutCustomerDelivered();
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
        _customerListRepository.CustomerCabRequest(new Customer("Dan", "1 Fulton Drive", "2 Destination Lane"));
        _customerListRepository.CancelledCall();
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