using Production.EmmaCabCompany.Application;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CustomerCancelledCabHandler : ICustomerCancelledCabHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerCancelledCabHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }

    public int Handle(CustomerCancelledCab request)
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.CancelPickup();
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}
public interface ICustomerCancelledCabHandler
{
    public int Handle(CustomerCancelledCab addCabCommand);
}