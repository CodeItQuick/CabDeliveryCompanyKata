using Production.EmmaCabCompany.Application;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CustomerCabRequestedHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerCabRequestedHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle(CustomerCabRequested request)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.CustomerCabCall(
            new Customer(request.CustomerName, request.StartLocation, request.EndLocation));
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}