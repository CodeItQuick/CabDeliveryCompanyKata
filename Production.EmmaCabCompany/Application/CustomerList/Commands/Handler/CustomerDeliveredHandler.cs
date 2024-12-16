using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

public class CustomerDeliveredHandler : ICustomerDeliveredHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerDeliveredHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle<TS>(TS @event) where TS : CustomerDelivered
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.CustomerDelivered();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}
public interface ICustomerDeliveredHandler : IHandler<CustomerDelivered>;