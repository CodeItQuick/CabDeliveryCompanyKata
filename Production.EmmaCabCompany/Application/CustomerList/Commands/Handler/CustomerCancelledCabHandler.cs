using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

public class CustomerCancelledCabHandler : ICustomerCancelledCabHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerCancelledCabHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle<TS>(TS @event) where TS : CustomerCancelledCab
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.CancelPickup();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}
public interface ICustomerCancelledCabHandler : IHandler<CustomerCancelledCab>;