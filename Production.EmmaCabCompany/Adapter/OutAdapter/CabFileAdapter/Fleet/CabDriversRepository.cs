using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

public class CabDriversRepository : ICabDriversRepository
{
    private CabContext _cabContext;

    public CabDriversRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public void Save(Domain.Fleet.Fleet entity)
    {
        foreach (var entityCab in entity.Cabs)
        {
            _cabContext.CabDrivers.Add(new CabDriver()
            {
                _cabName = entityCab._cabName,
                _latitude = entityCab._latitude,
                _longitude = entityCab._longitude,
                _wallet = entityCab._wallet,
                Customer = new Customer() { Id = entityCab.CustomerId }
            });
        }

        if (entity.Cabs.Any())
        {
            _cabContext.SaveChanges();
        }
    }

    public void Remove(int entityId)
    {
        try
        {
            var cab = _cabContext.CabDrivers
                .Where(x => x.Id == entityId && x.IsStatus(CabStatus.Available))
                .ToList();
            _cabContext.CabDrivers.RemoveRange(cab);
            _cabContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Cab cannot be removed until passenger dropped off.");
        }
    }

    public Domain.Fleet.Fleet GetById(int? customerId)
    {
        var fleetDto = _cabContext.CabDrivers
            .Where(x => x.Customer.Id == customerId)
            .ToList()
            .Select(x => new Cab(x._cabName, x._wallet, x._latitude, x._longitude)
            {
                Id = x.Id, CustomerId = x.Customer?.Id ?? 0,
                _status = CabStatus.TryParse(x._status.ToString(), out Domain.Fleet.CabStatus cabStatus)
                    ? cabStatus
                    : Domain.Fleet.CabStatus.Available,
                _cabName = x._cabName,
                _latitude = x._latitude,
                _longitude = x._longitude,
            })
            .ToList();
        _cabContext.ChangeTracker.Clear();
        return new Domain.Fleet.Fleet() { Cabs = fleetDto };
    }
}