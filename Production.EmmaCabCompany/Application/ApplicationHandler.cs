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

namespace Production.EmmaCabCompany.Application;

public class ApplicationHandler : IApplicationHandler
{
    private readonly ICabDriversRepository _cabDriversRepository;
    private readonly IPatronRepository _patronRepository;
    private readonly CabDriverRepository _cabDriverRepository;
    private ServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;
    private CustomerRepository _customerRepository;
    private Domain.Customers.Customer _currentFleet;

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
        _cabDriverRepository = new CabDriverRepository(cabContext);
        _customerRepository = new CustomerRepository(cabContext);
    }

    public void Handle(NewUserRegistered @event)
    {
        _customerRepository.Save(new Domain.Customers.Customer());
    }

    public void Handle(UserLogin @event)
    {
        _currentFleet = _customerRepository.GetById(@event.Id);
    }

    public void Handle(CustomerCabRequested @event)
    {
        var patron = _patronRepository.GetById(1);
        var fleet = _cabDriversRepository.GetById(@event.CustomerId);
        var transportingCab = fleet.Cabs.FirstOrDefault(x => x.IsStatus(Domain.Fleet.CabStatus.Available));
        transportingCab.RequestRideFor(patron);
        _cabDriverRepository.Save(transportingCab);
        _patronRepository.Save(patron);
    }

    public void Handle(CustomerCancelledCab @event)
    {
        // var customerList = _patronRepository.GetById(new Domain.CustomerList.PatronList() { Id = 1 });
        // customerList.CancelPickup();
        // var customer = customerList.Customers.FirstOrDefault(x => x.Status == PatronStatus.CustomerCallInProgress);
        // _patronRepository.Save(customer);
    }

    public void Handle(CustomerDelivered @event)
    {
        // var customerList = _patronRepository.GetById(new Domain.CustomerList.PatronList() { Id = 1 });
        // customerList.CustomerDelivered();
        // var customer = customerList.Customers.FirstOrDefault(x => x.Status == PatronStatus.Delivered);
        // _patronRepository.Save(customer);
    }

    public void Handle(CustomerPickedUp @event)
    {
        // var customerList = _patronRepository.GetById(new Domain.CustomerList.PatronList() { Id = 1 });
        // customerList.PickupCustomer();
        // var customer = customerList.Customers.FirstOrDefault(x => x.Status == PatronStatus.Enroute);
        // _patronRepository.Save(customer);
    }

    public void Handle(CustomerRideRequested @event)
    {
        // var customerList = _patronRepository.GetById(new Domain.CustomerList.PatronList { Id = 1 });
        // customerList.RideRequest();
        // var customer = customerList.Customers.FirstOrDefault(x => x.Status == PatronStatus.WaitingPickup);
        // _patronRepository.Save(customer);
    }

    public void Handle(AddCabCommand @event)
    {
        // var currentFleet = _cabDriversRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet()
        //     { Id = @event.UserId });
        // var cab = new CabDriver(
        //     @event.CabName,
        //     0,
        //     @event.Latitude,
        //     @event.Longitude) { Customer = currentFleet};
        // _cabDriversRepository.Save(cab);
        //
        // _currentFleet = _cabDriversRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet()
        //     { Id = @event.UserId });
        // _menuRepository.Save(new Adapter.OutAdapter.CabFileAdapter.Menu.Menu()
        // {
        //     Id = @event.UserId,
        //     Fleets = [_currentFleet]
        // });
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