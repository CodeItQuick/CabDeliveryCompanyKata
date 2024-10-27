using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CustomerListRepository : ICustomerListRepository
{
    private CabContext _cabContext;

    public CustomerListRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
        EnsureFleetExistsForSingleUser();
    }

    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.CustomerList.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.CustomerList.Add(new CustomerList() { Id = 1 });
        _cabContext.SaveChanges();
    }
    public void CustomerCabRequest(Customer customer)
    {
        var customerList = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()!;
        customerList.CustomerCabCall(customer);
        _cabContext.CustomerList.Update(customerList);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void RideRequest()
    {
        var customerList = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()!;
        customerList.RideRequest();
        _cabContext.CustomerList.Update(customerList);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void PickupCustomer()
    {
        var customerList = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()!;
        customerList.PickupCustomer();
        _cabContext.CustomerList.Update(customerList);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void PutCustomerEnroute()
    {
        var customerList = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()!;
        customerList.PutCustomerEnroute();
        _cabContext.CustomerList.Update(customerList);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void PutCustomerDelivered()
    {
        var customerList = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()!;
        customerList.PutCustomerDelivered();
        _cabContext.CustomerList.Update(customerList);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void CancelledCall()
    {
        var customerList = _cabContext.CustomerList
            .Include(x => x.Customers)
            .FirstOrDefault()!;
        customerList.CancelPickup();
        _cabContext.CustomerList.Update(customerList);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }
}