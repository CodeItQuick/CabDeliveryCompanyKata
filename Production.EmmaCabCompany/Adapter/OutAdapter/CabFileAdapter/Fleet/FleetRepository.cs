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

    public void Save(Fleet entity)
    {
        if (entity.Id == null)
        {
            _cabContext.Fleet.Add(entity);
        }
        else if (entity.FleetOfCabs.Count != 0)
        {
            _cabContext.CabDrivers.AttachRange(entity.FleetOfCabs);
            var find = _cabContext.Fleet.Find(entity.Id);
            if (find != null)
            {
                _cabContext.Fleet.Update(find);
            };
        }
        _cabContext.SaveChanges();
    }

    public void Remove(int entityId)
    {
        try
        {
            var fleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
                .FirstOrDefault(x => x.Id == entityId);
            if (fleet!.FleetOfCabs.FirstOrDefault()!.IsStatus(CabStatus.Available))
            {
                _cabContext.CabDrivers.Remove(fleet!.FleetOfCabs.FirstOrDefault(x => x.IsStatus(CabStatus.Available))!);
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

    public Fleet GetById(Fleet entity)
    {
        var fleetDto = _cabContext.Fleet.Include(fleet => fleet.FleetOfCabs).FirstOrDefault(x => x.Id == entity.Id);
        return new Fleet() { Id = fleetDto.Id, FleetOfCabs = fleetDto?.FleetOfCabs ?? new List<CabDriver>() };
    }
}