using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

public class FleetRepository : IFleetRepository, IDisposable
{
    private CabContext _cabContext;

    public FleetRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public void Save(Cab cab)
    {
        var updateFleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
            .FirstOrDefault(x => x.Id == 1);
        var cab1 = new CabDriver(cab._cabName, cab._wallet, cab._latitude, cab._longitude);
        if (updateFleet == null)
        {
            updateFleet = new Fleet();
            _cabContext.Fleet.Add(updateFleet);
            _cabContext.SaveChanges();
        }
        updateFleet.FleetOfCabs.Add(cab1);
        _cabContext.CabDrivers.Add(cab1);
        _cabContext.SaveChanges();
        _cabContext.Fleet.Update(updateFleet);
        _cabContext.SaveChanges();
        var updateMenu = _cabContext.Menu
            .Include(x => x.Customers)
            .Include(x => x.Cabs)
            .FirstOrDefault(x => x.Id == 1)!;
        updateMenu.Cabs.Add(cab1);
        _cabContext.Menu.Update(updateMenu);
        _cabContext.SaveChanges();
    }

    public void Remove(int fleetId)
    {
        try
        {
            var fleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
                .FirstOrDefault(x => x.Id == fleetId);
            _cabContext.CabDrivers.Remove(fleet!.FleetOfCabs.FirstOrDefault()!);
            _cabContext.SaveChanges();
        }
        catch (ArgumentNullException)
        {
            throw new Exception("Cab cannot be removed until passenger dropped off.");
        }
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