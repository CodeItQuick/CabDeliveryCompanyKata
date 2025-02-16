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
    public void CanEmptyFleet()
    {
        _fleetRepository.EmptyFleet(1);
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Empty(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }
    [Fact]
    public void CanAddCab()
    {
        _fleetRepository.EmptyFleet(1);
        _fleetRepository.AddCab("evan", 1.00, 1.00);
        Assert.Single(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }
    [Fact]
    public void CanAddTwoCabs()
    {
        _fleetRepository.EmptyFleet(1);
        _fleetRepository.AddCab("evan", 1.00, 1.00);
        _fleetRepository.AddCab("dan", 1.00, 1.00);
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Equal(2, _cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs.Count);
    }
    [Fact]
    public void CanRemoveCab()
    {
        _fleetRepository.EmptyFleet(1);
        _fleetRepository.AddCab("evan", 1.00, 1.00);
        _fleetRepository.RemoveCab(1);
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Empty(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }
    
}