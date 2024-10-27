using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class FleetRepository : IFleetRepository
{
    private CabContext _cabContext;

    public FleetRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
        EnsureFleetExistsForSingleUser();
    }

    // TODO: this isn't actually adding a cab to the db
    public void AddCab(string cabName, double latitude, double longitude)
    {

        var updateFleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
            .FirstOrDefault(x => x.Id == 1)!;
        var cab = new Cab("Cab Driver Name", 0, 0, 0);
        updateFleet.AddCab(cab);
        _cabContext.Fleet.Update(updateFleet);
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.Fleet.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Fleet.Add(new Fleet() { Id = 1 });
        _cabContext.SaveChanges();
    }

    public void RemoveCab(int fleetId)
    {
        try
        {
            var fleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
                .FirstOrDefault(x => x.Id == fleetId);
            _cabContext.Cabs.Remove(fleet!.SignedOutCab());
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