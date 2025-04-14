using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

public class CabDriverRepository : ICabDriverRepository, IDisposable
{
    private CabContext _cabContext;

    public CabDriverRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public void Save(Cab entity)
    {
        var customer = new Customer() { Id = entity.CustomerId };
        var cabDriver = new CabDriver()
        {
            Id = entity.Id,
            Customer = customer,
            _cabName = entity._cabName,
            _latitude = entity._latitude,
            _longitude = entity._longitude,
            _status = CabStatus.Available
        };
        if (cabDriver.Id is null or 0)
        {
            _cabContext.Customers.Attach(customer);
            _cabContext.Add(cabDriver);
            _cabContext.Entry(cabDriver).State = EntityState.Unchanged;
            _cabContext.SaveChanges();
            _cabContext.ChangeTracker.Clear();
        }
        else if (entity.Id != 0)
        {
            _cabContext.CabDrivers.Attach(cabDriver);
            var find = _cabContext.CabDrivers.Find(entity.Id);
            if (find != null)
            {
                _cabContext.CabDrivers.Update(cabDriver);
            };
            _cabContext.SaveChanges();
        }
    }

    public void Remove(int entityId)
    {
        try
        {
            var cab = _cabContext.CabDrivers
                .FirstOrDefault(x => x.Id == entityId);
            if (cab!.IsStatus(CabStatus.Available))
            {
                _cabContext.CabDrivers.Remove(cab);
            }
            else
            {
                throw new ArgumentNullException();
            }
            _cabContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Cab cannot be removed until passenger dropped off.");
        }
    }

    public Cab GetById(int customerId)
    {
        var cabDriver = _cabContext.CabDrivers
            .Include(fleet => fleet.Customer)
            .FirstOrDefault(x => x.Id == customerId);
        return new Cab(cabDriver._cabName, cabDriver._wallet, cabDriver._latitude, cabDriver._longitude);
    }

    public void Dispose()
    {
        _cabContext.Dispose();
    }
}