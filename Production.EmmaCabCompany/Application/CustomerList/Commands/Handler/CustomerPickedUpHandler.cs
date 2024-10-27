using Production.EmmaCabCompany.Application;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CustomerPickedUpHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerPickedUpHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle(CustomerPickedUp request)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.PickupCustomer();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}