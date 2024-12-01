namespace Production.EmmaCabCompany.Application.CustomerList;

public interface ICustomerListRepository
{
    public Domain.CustomerList.CustomerList GetById(int customerListId);
    public void Add(Domain.CustomerList.CustomerList customerList);
}