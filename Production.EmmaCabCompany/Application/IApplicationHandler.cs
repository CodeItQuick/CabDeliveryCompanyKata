using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;
using Production.EmmaCabCompany.Application.Fleet;

namespace Production.EmmaCabCompany.Application;

public interface IApplicationHandler :
    ICustomerCabRequestedCommandHandler,
    ICustomerCancelledCabCommandHandler,
    ICustomerPickedUpCommandHandler,
    ICustomerDeliveredCommandHandler,
    IAddCabCommandHandler,
    IRemoveCabCommandCommandHandler,
    ICustomerRideRequestedCommandHandler;

public interface ICommandHandler<in T> where T : IEvent
{
    void Handle(T @event);
}

public interface IEvent;