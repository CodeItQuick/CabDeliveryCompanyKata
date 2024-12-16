using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

public class CustomerCabRequestedHandler : ICustomerCabRequestedHandler
{
    private readonly ICustomerListRepository _customerListRepository;

    // TODO: should be internal, but then how to test?
    public CustomerCabRequestedHandler(
        ICustomerListRepository customerListRepository)
    {
        _customerListRepository = customerListRepository;
    }
    
    public int Handle<TS>(TS request) where TS : CustomerCabRequested
    {
        var customerList = _customerListRepository.GetById(1);
        customerList.CustomerCabCall(
            new Customer(request.CustomerName, request.StartLocation, request.EndLocation));
        _customerListRepository.Add(customerList);

        return customerList.Id;
    }
}


public interface ICustomerCabRequestedHandler : IHandler<CustomerCabRequested>;
