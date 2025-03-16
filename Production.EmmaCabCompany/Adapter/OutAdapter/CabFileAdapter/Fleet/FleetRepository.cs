using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

public class FleetRepository : IFleetRepository
{
    private CabContext _cabContext;

    public FleetRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public void Save(Cab cab)
    {
        var updateFleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
            .FirstOrDefault(x => x.Id == 1)!;
        var cab1 = new CabDto("Cab Driver Name", 0, 0, 0);
        updateFleet.FleetOfCabs.Add(cab1);
        _cabContext.Fleet.Update(updateFleet);
        _cabContext.SaveChanges();
        var updateMenu = _cabContext.Menu
            .Include(x => x.Customers)
            .Include(x => x.Cabs)
            .FirstOrDefault(x => x.Id == 1)!;
        updateMenu.Cabs.Add(cab1);
        _cabContext.Menu.Update(updateMenu);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void Remove(int fleetId)
    {
        try
        {
            var fleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
                .FirstOrDefault(x => x.Id == fleetId);
            _cabContext.Cabs.Remove(fleet!.FleetOfCabs.FirstOrDefault()!);
            _cabContext.SaveChanges();
        }
        catch (ArgumentNullException)
        {
            throw new Exception("Cab cannot be removed until passenger dropped off.");
        }
    }
}