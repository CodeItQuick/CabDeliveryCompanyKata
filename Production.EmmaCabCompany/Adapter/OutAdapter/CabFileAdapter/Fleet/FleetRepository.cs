using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Application.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class FleetRepository : IFleetRepository
{
    private CabContext _cabContext;

    public FleetRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
        EnsureFleetExistsForSingleUser();
        EnsureMenuExistsForSingleUser();
    }

    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.Fleet.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Fleet.Add(new Fleet.Fleet() { Id = 1 });
        _cabContext.SaveChanges();
    }

    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Menu.Menu() { Id = 1 });
        _cabContext.SaveChanges();
    }

    public void AddCab(string cabName, double latitude, double longitude)
    {

        var updateFleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
            .FirstOrDefault(x => x.Id == 1)!;
        var cab = new CabDto("Cab Driver Name", 0, 0, 0);
        updateFleet.FleetOfCabs.Add(cab);
        _cabContext.Fleet.Update(updateFleet);
        _cabContext.SaveChanges();
        var updateMenu = _cabContext.Menu
            .Include(x => x.Customers)
            .Include(x => x.Cabs)
            .FirstOrDefault(x => x.Id == 1)!;
        updateMenu.Cabs.Add(cab);
        _cabContext.Menu.Update(updateMenu);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void RemoveCab(int fleetId)
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

    public void EmptyFleet(int fleetId)
    {
        var fleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
            .FirstOrDefault(x => x.Id == fleetId);
        _cabContext.Cabs.RemoveRange(fleet!.FleetOfCabs.ToList());
        _cabContext.SaveChanges();
    }
}