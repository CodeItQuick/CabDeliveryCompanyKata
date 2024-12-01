using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;

public class CustomerListRepository : ICustomerListRepository
{
    private CabContext _cabContext;

    public CustomerListRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
        EnsureMenuExistsForSingleUser();
        EnsureFleetExistsForSingleUser();
    }

    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Menu.Menu() { Id = 1, Customers = new List<CustomerDto>()});
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.CustomerList.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.CustomerList.Add(new CustomerListDto() { Id = 1, Customers = new List<CustomerDto>() });
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public Domain.CustomerList.CustomerList GetById(int customerListId)
    {
        var customerListDto = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault(x => x.Id == customerListId)!;
        var customers = customerListDto.Customers.Select(x =>
            new Customer(x.Name, x.StartLocation!, x.EndLocation) { Status = x.Status }).ToList();
        var customerList = Domain.CustomerList.CustomerList.CreateCustomerList(customerListDto.Id,
            customers);
        return customerList;
    }

    public void Add(Domain.CustomerList.CustomerList customerList)
    {
        var customerListDto = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault(x => x.Id == customerList.Id)!;
        var customerDtos = customerList.Customers
            .Select(x => new CustomerDto()
            {
                CustomerId = customerList.Id, Id = x.Id, Name = x.Name, Status = x.Status, EndLocation = x.EndLocation,
                StartLocation = x.StartLocation, MenuId = 1
            }).ToList();
        customerListDto.Customers = customerDtos;
        _cabContext.Update(customerListDto); 
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }
}