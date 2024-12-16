using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

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
    private readonly List<Func<IEvent, int>> handler = new();

    public ApplicationHandler(
        ICustomerCabRequestedHandler customerCabRequested, 
        ICustomerCancelledCabHandler customerCancelledCabRequestedHandler)
    {
        handler.Add(@event =>
        {
            if (@event.GetType() == typeof(CustomerCabRequested))
            {
                return customerCabRequested.Handle<CustomerCabRequested>(@event as CustomerCabRequested);
            }

            return 0;
        });
        handler.Add(@event =>
        {
            if (@event.GetType() == typeof(CustomerCancelledCab))
            {
                return customerCancelledCabRequestedHandler.Handle<CustomerCancelledCab>(@event as CustomerCancelledCab);
            }

            return 0;
        });
        
    }

    public int Handle<T>(T request) where T : IEvent
    {
        handler.ForEach(x => x.Invoke(request));
        return 1;
    }
}

public interface IEvent;