using Production.EmmaCabCompany.Application;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CustomerDeliveredHandler : ICustomerDeliveredHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerDeliveredHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle(CustomerDelivered request)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.PutCustomerDelivered();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}
public interface ICustomerDeliveredHandler
{
    public int Handle(CustomerDelivered request);
}