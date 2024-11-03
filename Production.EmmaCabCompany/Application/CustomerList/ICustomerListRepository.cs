using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Application;

public interface ICustomerListRepository
{
    public CustomerList GetById(int customerListId);
    public void Add(CustomerList customerList);
}