using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

public class CustomerRideRequestedHandler : ICustomerRideRequestedHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerRideRequestedHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle(CustomerRideRequested request)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.RideRequest();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}
public interface ICustomerRideRequestedHandler
{
    public int Handle(CustomerRideRequested request);
}