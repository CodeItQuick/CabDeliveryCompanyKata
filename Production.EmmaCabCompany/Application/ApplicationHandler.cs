using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Application;

public class ApplicationHandler : IApplicationHandler
{
    private readonly IFleetRepository _fleetRepository;
    private readonly ICustomerListRepository _customerListRepository;
    private Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet _currentFleet = new() { Id = 1};
    private MenuRepository _menuRepository;

    public ApplicationHandler(CabContext cabContext)
    {
        _fleetRepository = new FleetRepository(cabContext);
        _customerListRepository = new CustomerListRepository(cabContext);
        _menuRepository = new MenuRepository(cabContext);
    }

    public void Handle(NewUserRegistered @event)
    {
        var fleet = new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = null };
        _fleetRepository.Save(fleet);
        var fleetCoordinator = new FleetCoordinator();
        _fleetRepository.Save(fleetCoordinator);
        _menuRepository.Save(new Adapter.OutAdapter.CabFileAdapter.Menu.Menu());
    }
    public void Handle(UserLogin @event)
    {
        var fleet = new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = @event.Id };
        _currentFleet = _fleetRepository.GetFleetById(fleet);
    }

    public void Handle(CustomerCabRequested @event)
    {
        var customerList = _customerListRepository.GetById(1); // separately managed
        customerList.CustomerCabCall(
            new Customer(@event.CustomerName, @event.StartLocation, @event.EndLocation));
        _customerListRepository.Save(customerList); // write
    }
    public void Handle(CustomerCancelledCab @event)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.CancelPickup();
        _customerListRepository.Save(customerList);
    }
    public void Handle(CustomerDelivered @event)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.CustomerDelivered();
        _customerListRepository.Save(customerList);
    }
    public void Handle(CustomerPickedUp @event)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.PickupCustomer();
        _customerListRepository.Save(customerList);
    }
    public void Handle(CustomerRideRequested @event)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.RideRequest();
        _customerListRepository.Save(customerList);
    }
    public void Handle(AddCabCommand @event)
    {
        var cab = new Cab(
            @event.CabName, 
            0, 
            @event.Latitude, 
            @event.Longitude) { Fleet = new Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet() { Id = @event.UserId }};
        _fleetRepository.Save(cab);
        _menuRepository.Save(new Adapter.OutAdapter.CabFileAdapter.Menu.Menu() { Id = 1, Cabs = _currentFleet.FleetOfCabs.ToList() });
    }
    
    public void Handle(RemoveCabCommand @event)
    {
        _fleetRepository.Remove(_currentFleet.Id ?? 1);
    }
}