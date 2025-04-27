using Microsoft.Extensions.DependencyInjection;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Customers;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Patrons;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;
using Production.EmmaCabCompany.Domain.Fleet;
using CabStatus = Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet.CabStatus;
using Customer = Production.EmmaCabCompany.Domain.Customers.Customer;

namespace Production.EmmaCabCompany.Application;

public class ApplicationHandler : IApplicationHandler
{
    private readonly ICabDriversRepository _cabDriversRepository;
    private readonly IPatronRepository _patronRepository;
    private readonly CabDriverRepository _cabDriverRepository;
    private PatronsRepository _patronsRepository;
    private ServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;
    private CustomerRepository _customerRepository;
    private Customer _currentFleet;

    private Dictionary<Type, Type> handlers { get; } = new()
    {
        [typeof(AddCabCommand)] = typeof(AddCabCommandHandler)
    };

    public void Send<T>(T command) where T : IEvent
    {
        var handlerType = handlers[typeof(T)];
        var handler = (ICommandHandler<T>)_serviceProvider.GetRequiredService(handlerType);
        handler.Handle(command);
    }

    public ApplicationHandler(CabContext cabContext)
    {
        _serviceCollection = [];
        _serviceCollection.AddScoped<ICommandHandler<AddCabCommand>, AddCabCommandHandler>();
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _cabDriversRepository = new CabDriversRepository(cabContext);
        _patronRepository = new PatronRepository(cabContext);
        _patronsRepository = new PatronsRepository(cabContext);
        _cabDriverRepository = new CabDriverRepository(cabContext);
        _customerRepository = new CustomerRepository(cabContext);
    }

    public void Handle(NewUserRegistered @event)
    {
        _customerRepository.Save(new Domain.Customers.Customer());
    }

    public Customer Handle(UserLogin @event)
    {
        return _customerRepository.GetById(@event.Id);
    }

    public void Handle(CustomerCabRequested @event)
    {
        var entity = new Patron(@event.CustomerName, @event.StartLocation, @event.EndLocation)
        {
            CustomerId = @event.CustomerId,
            Status = PatronStatus.CustomerCallInProgress
        };
        _patronRepository.Save(entity);
        var patron = _patronsRepository
            .GetById(@event.CustomerId)
            .Patrons.FirstOrDefault(x => x.Status == PatronStatus.CustomerCallInProgress);
        var fleet = _cabDriversRepository.GetById(@event.CustomerId);
        var transportingCab = fleet.Cabs.FirstOrDefault(x => x.IsStatus(Domain.Fleet.CabStatus.Available));
        transportingCab.RequestRideFor(patron);
        _cabDriverRepository.Save(transportingCab);
    }

    public void Handle(CustomerCancelledCab @event)
    {
        // var customerList = _patronRepository.GetById(new Domain.CustomerList.PatronList() { Id = 1 });
        // customerList.CancelPickup();
        // var customer = customerList.Customers.FirstOrDefault(x => x.Status == PatronStatus.CustomerCallInProgress);
        // _patronRepository.Save(customer);
    }

    public void Handle(CustomerRideRequested @event)
    {
        var customerList = _patronsRepository.GetById(@event.CustomerListId);
        var customer = customerList.Patrons.FirstOrDefault(x => x.Status == PatronStatus.CustomerCallInProgress);
        customer.Status = PatronStatus.WaitingPickup;
        _patronRepository.Save(customer);
        var fleet = _cabDriversRepository.GetById(@event.CustomerListId);
        var transportingCab = fleet.Cabs.FirstOrDefault(x => x.IsStatus(Domain.Fleet.CabStatus.CustomerRideRequested));
        transportingCab?.PickupAssignedCustomer(customer);
        _cabDriverRepository.Save(transportingCab);
    }

    public void Handle(CustomerPickedUp @event)
    {
        var customerList = _patronsRepository.GetById(@event.CustomerListId);
        customerList.PickupCustomer();
        var customer = customerList.Patrons.FirstOrDefault(x => x.Status == PatronStatus.Enroute);
        _patronRepository.Save(customer);
    }

    public void Handle(CustomerDelivered @event)
    {
        var customerList = _patronsRepository.GetById(@event.CustomerListId);
        customerList.CustomerDelivered();
        var customer = customerList.Patrons.FirstOrDefault(x => x.Status == PatronStatus.Delivered);
        _patronRepository.Save(customer);

        var cab = _cabDriversRepository.GetById(@event.CustomerListId)
            .Cabs.FirstOrDefault(x => x._status == Domain.Fleet.CabStatus.TransportingCustomer);
        cab._status = Domain.Fleet.CabStatus.Available;
        _cabDriverRepository.Save(cab);
    }

    public void Handle(AddCabCommand @event)
    {
        var cab = new Cab(@event.CabName, 0, @event.Latitude, @event.Longitude) {
            CustomerId = @event.UserId
        };
        _cabDriverRepository.Save(cab);
    }

    public void Handle(RemoveCabCommand @event)
    {
        // var currentFleet = _cabDriversRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet()
        //     { Id = @event.UserId });
        //
        // var removeCab = new CabDriver() { Customer = currentFleet };
        // _cabDriversRepository.Save(removeCab);
        //
        // _currentFleet = _cabDriversRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet()
        //     { Id = @event.UserId });
        // _menuRepository.Save(new Adapter.OutAdapter.CabFileAdapter.Menu.Menu()
        // {
        //     Id = @event.UserId,
        //     Fleets = [_currentFleet]
        // });
    }
}