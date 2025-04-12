using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class FleetRepositoryTests
{
    private readonly CabContext _cabContext;
    private FleetRepository _fleetRepository;

    public FleetRepositoryTests()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");
        _cabContext = new CabContext(dbContextOptionsBuilder.Options);
        _cabContext.Database.Migrate();
        _fleetRepository = new FleetRepository(_cabContext);
    }
    [Fact]
    public void CanAddCab()
    {
        var cab = new CabDriver("evan", 1, 1.00, 1.00);
        var fleet = new Fleet();
        _fleetRepository.Save(fleet);
        fleet.FleetOfCabs.Add(cab);
        
        _fleetRepository.Save(fleet);
        
        Assert.Single(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }
    [Fact]
    public void CanAddTwoCabs()
    {
        var cabOne = new CabDriver("evan", 1, 1.00, 1.00);
        var cabTwo = new CabDriver("dan", 1, 1.00, 1.00);
        var fleet = new Fleet() { FleetOfCabs = new List<CabDriver>() { cabOne, cabTwo } };
        
        _fleetRepository.Save(fleet);
        
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Equal(2, _cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs.Count);
    }
    [Fact]
    public void CanRemoveCab()
    {
        var cabOne = new CabDriver("evan", 1, 1.00, 1.00);
        var fleet = new Fleet();
        _fleetRepository.Save(fleet);
        fleet.FleetOfCabs.Add(cabOne);
        _fleetRepository.Save(fleet);
        
        _fleetRepository.Remove(1);
        
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Empty(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }

}