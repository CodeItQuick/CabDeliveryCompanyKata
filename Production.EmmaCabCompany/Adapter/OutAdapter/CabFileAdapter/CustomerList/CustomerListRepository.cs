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
    }

    public Domain.CustomerList.CustomerList GetById(int customerListId)
    {
        var customerListDto = _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault(x => x.Id == customerListId)!;
        var customers = customerListDto.Patrons.Select(x =>
            new Customer(x.Name, x.StartLocation!, x.EndLocation) { Status = x.Status }).ToList();
        var customerList = Domain.CustomerList.CustomerList.CreateCustomerList(customerListDto.Id,
            customers);
        return customerList;
    }

    public void Save(Domain.CustomerList.CustomerList customerList)
    {
        var fleetCoordinator = _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault(x => x.Id == customerList.Id)!;
        var patrons = customerList.Customers
            .Select(x => new PatronDto()
            {
                Id = x.Id, Name = x.Name, Status = x.Status, EndLocation = x.EndLocation,
                StartLocation = x.StartLocation, MenuId = 1
            }).ToList();
        fleetCoordinator.Patrons = patrons;
        _cabContext.Patron.AddRange(patrons);
        _cabContext.SaveChanges();
        _cabContext.Update(fleetCoordinator); 
        _cabContext.SaveChanges();
    }
}