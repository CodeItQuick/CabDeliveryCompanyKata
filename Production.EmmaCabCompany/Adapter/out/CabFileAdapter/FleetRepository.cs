

using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Adapter.@out.CabFileAdapter;

public class FleetRepository : IFleetRepository
{
    private CabContext _cabContext;

    public FleetRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
        EnsureFleetExistsForSingleUser();
    }

    public void AddCab(string cabName, double latitude, double longitude)
    {

        var fleet = _cabContext.Fleet.FirstOrDefault(x => x.Id == 1);
        fleet?.AddCab(new Cab(cabName, 20, latitude, longitude));
    }

    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.Fleet.Any(x => x.Id == 1);
        if (!fleetExists)
        {
            _cabContext.Fleet.Add(new Fleet() { Id = 1 });
            _cabContext.SaveChanges();
        }
    }
}