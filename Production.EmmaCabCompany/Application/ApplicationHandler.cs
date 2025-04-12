using Microsoft.Extensions.DependencyInjection;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;
using Production.EmmaCabCompany.Domain.Fleet;
using CabStatus = Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet.CabStatus;

namespace Production.EmmaCabCompany.Application;

public class ApplicationHandler : IApplicationHandler
{
    private readonly IFleetRepository _fleetRepository;
    private readonly ICustomerListRepository _customerListRepository;
    private Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet _currentFleet = new() { Id = 1};
    private MenuRepository _menuRepository;
    private ServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;

    private Dictionary<Type, Type> handlers { get; } = new()
    {
        [typeof(AddCabCommand)] = typeof(AddCabCommandHandler)
    };

    public void Send<T>(T command) where T : IEvent
    {
        var handlerType = handlers[typeof(T)];
        var handler = (ICommandHandler<T>) _serviceProvider.GetRequiredService(handlerType);
        handler.Handle(command);
    }

    public ApplicationHandler(CabContext cabContext)
    {
        _serviceCollection = [];
        _serviceCollection.AddScoped<ICommandHandler<AddCabCommand>, AddCabCommandHandler>();
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _fleetRepository = new FleetRepository(cabContext);
        _customerListRepository = new CustomerListRepository(cabContext);
        _menuRepository = new MenuRepository(cabContext);
    }

    public void Handle(NewUserRegistered @event)
    {
        var fleet = new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = null };
        _fleetRepository.Save(fleet);
        var fleetCoordinator = new FleetCoordinator();
        _customerListRepository.Save(new Domain.CustomerList.CustomerList() {  });
        _menuRepository.Save(new Adapter.OutAdapter.CabFileAdapter.Menu.Menu());
    }
    public void Handle(UserLogin @event)
    {
        var fleet = new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = @event.Id };
        _currentFleet = _fleetRepository.GetById(fleet);
    }

    public void Handle(CustomerCabRequested @event)
    {
        var customerList = _customerListRepository.GetById(new Domain.CustomerList.CustomerList() { Id = 1 }); // separately managed
        customerList.CustomerCabCall(
            new Customer(@event.CustomerName, @event.StartLocation, @event.EndLocation));
        _customerListRepository.Save(customerList); // write
    }
    public void Handle(CustomerCancelledCab @event)
    {
        var customerList = _customerListRepository.GetById(new Domain.CustomerList.CustomerList() { Id = 1 });
        customerList.CancelPickup();
        _customerListRepository.Save(customerList);
    }
    public void Handle(CustomerDelivered @event)
    {
        var customerList = _customerListRepository.GetById(new Domain.CustomerList.CustomerList() { Id = 1 });
        customerList.CustomerDelivered();
        _customerListRepository.Save(customerList);
    }
    public void Handle(CustomerPickedUp @event)
    {
        var customerList = _customerListRepository.GetById(new Domain.CustomerList.CustomerList() { Id = 1 });
        customerList.PickupCustomer();
        _customerListRepository.Save(customerList);
    }
    public void Handle(CustomerRideRequested @event)
    {
        var customerList = _customerListRepository.GetById(new Domain.CustomerList.CustomerList() { Id = 1 });
        customerList.RideRequest();
        _customerListRepository.Save(customerList);
    }
    public void Handle(AddCabCommand @event)
    {
        var currentFleet = _fleetRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = @event.UserId });
        var cab = new CabDriver(
            @event.CabName, 
            0, 
            @event.Latitude, 
            @event.Longitude);
        currentFleet.FleetOfCabs.Add(cab);
        _fleetRepository.Save(currentFleet);
        
        _currentFleet = _fleetRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = @event.UserId });
        _menuRepository.Save(new Adapter.OutAdapter.CabFileAdapter.Menu.Menu() { Id = @event.UserId, Cabs = _currentFleet.FleetOfCabs.ToList() });
    }
    
    public void Handle(RemoveCabCommand @event)
    {
        var currentFleet = _fleetRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = @event.UserId });

        var removeCab = currentFleet.FleetOfCabs.FirstOrDefault(x => x.IsStatus(CabStatus.Available))!;
        currentFleet.FleetOfCabs.Remove(removeCab);
        _fleetRepository.Save(currentFleet);
        
        _currentFleet = _fleetRepository.GetById(new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = @event.UserId });
        _menuRepository.Save(new Adapter.OutAdapter.CabFileAdapter.Menu.Menu() { Id = @event.UserId, Cabs = _currentFleet.FleetOfCabs.ToList() });
    }
}