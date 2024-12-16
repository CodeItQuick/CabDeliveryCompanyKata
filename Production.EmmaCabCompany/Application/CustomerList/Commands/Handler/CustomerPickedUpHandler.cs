using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

public class CustomerPickedUpHandler : ICustomerPickedUpHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerPickedUpHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle<TS>(TS request) where TS : CustomerPickedUp
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.PickupCustomer();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}
public interface ICustomerPickedUpHandler : IHandler<CustomerPickedUp>;