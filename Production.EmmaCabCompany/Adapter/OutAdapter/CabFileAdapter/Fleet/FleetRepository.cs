using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
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
            .FirstOrDefault(x => x.Id == cab.Fleet.Id);
        var cab1 = new CabDriver(cab._cabName, cab._wallet, cab._latitude, cab._longitude);
        if (updateFleet == null)
        {
            updateFleet = new Fleet() { Id = cab.Fleet.Id ?? 1 };
            _cabContext.Fleet.Add(updateFleet);
            _cabContext.SaveChanges();
        }

        var savedCab = _cabContext.CabDrivers.Add(cab1);
        _cabContext.SaveChanges();
        updateFleet.FleetOfCabs.Add(savedCab.Entity);
        _cabContext.Fleet.Update(updateFleet);
        _cabContext.SaveChanges();
    }

    public void Save(Fleet fleet)
    {
        _cabContext.Fleet.Add(fleet);
        _cabContext.SaveChanges();
    }

    public void Save(FleetCoordinator fleetCoordinator)
    {
        _cabContext.FleetCoordinator.Add(fleetCoordinator);
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
            _cabContext.ChangeTracker.Clear();
        }
        catch (ArgumentNullException)
        {
            throw new Exception("Cab cannot be removed until passenger dropped off.");
        }
    }

    public Fleet GetFleetById(Fleet fleet)
    {
        var fleetDto = _cabContext.Fleet.FirstOrDefault(x => x.Id == fleet.Id);
        return new Fleet() { Id = fleetDto.Id, FleetOfCabs = fleetDto?.FleetOfCabs ?? new List<CabDriver>()};
    }
}