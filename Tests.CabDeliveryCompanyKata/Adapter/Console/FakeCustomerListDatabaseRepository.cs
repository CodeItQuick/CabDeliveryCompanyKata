using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class FakeCustomerListDatabaseRepository : ICustomerListRepository
{
    public CustomerList GetById(int recordId)
    {
        throw new NotImplementedException();
    }

    public void Add(CustomerList customerList)
    {
        throw new NotImplementedException();
    }

    public CustomerList Read()
    {
        return null;
    }
}