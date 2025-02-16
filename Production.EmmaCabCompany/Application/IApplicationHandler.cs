using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;
using Production.EmmaCabCompany.Application.Fleet;

namespace Production.EmmaCabCompany.Application;

public interface IApplicationHandler
{
    int Handle<T>(T @event) where T : IEvent;
}

public interface IHandler<in T> where T : IEvent
{
    int Handle<TS>(TS @event) where TS : T;
}


public class ApplicationHandler : IApplicationHandler
{
    private readonly ICustomerCabRequestedHandler _customerCabRequested;
    private readonly ICustomerCancelledCabHandler _customerCancelledCabRequestedHandler;
    private readonly ICustomerPickedUpHandler _customerPickedUpHandler;
    private readonly ICustomerDeliveredHandler _customerDeliveredHandler;
    private readonly IAddCommandHandler _addCabCommandHandler;
    private readonly IRemoveCabCommandHandler _removeCabCommandHandler;

    public ApplicationHandler(
        ICustomerCabRequestedHandler customerCabRequested, 
        ICustomerCancelledCabHandler customerCancelledCabRequestedHandler,
        ICustomerPickedUpHandler customerPickedUpHandler,
        ICustomerDeliveredHandler customerDeliveredHandler, 
        IAddCommandHandler addCabCommandHandler,
        IRemoveCabCommandHandler removeCabCommandHandler)
    {
        _customerCabRequested = customerCabRequested;
        _customerCancelledCabRequestedHandler = customerCancelledCabRequestedHandler;
        _customerPickedUpHandler = customerPickedUpHandler;
        _customerDeliveredHandler = customerDeliveredHandler;
        _addCabCommandHandler = addCabCommandHandler;
        _removeCabCommandHandler = removeCabCommandHandler;
    }

    public int Handle<T>(T request) where T : IEvent
    {
        return request switch
        {
            CustomerCabRequested customerCabRequested => _customerCabRequested.Handle(customerCabRequested),
            CustomerCancelledCab customerCancelledCab => _customerCancelledCabRequestedHandler.Handle(
                customerCancelledCab),
            CustomerPickedUp customerPickedUp => _customerPickedUpHandler.Handle(customerPickedUp),
            CustomerDelivered customerDelivered => _customerDeliveredHandler.Handle(customerDelivered),
            AddCabCommand addCabCommand => _addCabCommandHandler.Handle(addCabCommand),
            RemoveCabCommand removeCabCommand => _removeCabCommandHandler.Handle(removeCabCommand),
            _ => throw new NotImplementedException("this handler isn't implemented")
        };
    }
}

public interface IEvent;