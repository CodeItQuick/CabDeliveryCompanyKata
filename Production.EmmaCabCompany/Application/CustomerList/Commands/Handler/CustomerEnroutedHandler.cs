using Production.EmmaCabCompany.Application;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CustomerEnroutedHandler : ICustomerEnroutedHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerEnroutedHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle(CustomerEnrouted request)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.PutCustomerEnroute();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}
public interface ICustomerEnroutedHandler
{
    public int Handle(CustomerEnrouted request);
}