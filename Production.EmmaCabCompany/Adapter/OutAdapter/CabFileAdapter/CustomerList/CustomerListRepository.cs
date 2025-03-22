using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;

public class CustomerListRepository : ICustomerListRepository, IDisposable
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
        var customerListDto = _cabContext.FleetCoordinator
            .Include(x => x.Patrons)
            .FirstOrDefault(x => x.Id == customerList.Id)!;
        var customerDtos = customerList.Customers
            .Select(x => new PatronDto()
            {
                CustomerId = customerList.Id, Id = x.Id, Name = x.Name, Status = x.Status, EndLocation = x.EndLocation,
                StartLocation = x.StartLocation, MenuId = 1
            }).ToList();
        customerListDto.Patrons = customerDtos;
        _cabContext.Update(customerListDto); 
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _cabContext.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}