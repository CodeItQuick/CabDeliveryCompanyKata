using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Application;

public class ApplicationHandler : IApplicationHandler, IDisposable
{
    private readonly IFleetRepository _fleetRepository;
    private readonly ICustomerListRepository _customerListRepository;
    
    public ApplicationHandler(CabContext cabContext)
    {
        _cabContext = cabContext;
        _fleetRepository = new FleetRepository(_cabContext);
        _customerListRepository = new CustomerListRepository(_cabContext);
    }

    public void Handle(CustomerCabRequested request)
    {
        var customerList = _customerListRepository.GetById(1); // separately managed
        customerList.CustomerCabCall(
            new Customer(request.CustomerName, request.StartLocation, request.EndLocation));
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
    public void Handle(CustomerPickedUp request)
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
    public void Handle(AddCabCommand request)
    {
        _fleetRepository.Save(new Cab(
            request.CabName, 
            0, 
            request.Latitude, 
            request.Longitude));
    }
    
    public void Handle(RemoveCabCommand addCabCommand)
    {
        _fleetRepository.Remove(addCabCommand.FleetId);
    }

    private bool disposed = false;
    private CabContext _cabContext;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _cabContext.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}