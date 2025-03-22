using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;

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
        EnsureFleetExistsForSingleUser();
        EnsureMenuExistsForSingleUser();
    }
    [Fact]
    public void CanEmptyFleet()
    {
        EmptyFleet(1);
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Empty(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }
    [Fact]
    public void CanAddCab()
    {
        EmptyFleet(1);
        var cab = new Cab("evan", 1, 1.00, 1.00);
        _fleetRepository.Save(cab);
        Assert.Single(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }
    [Fact]
    public void CanAddTwoCabs()
    {
        EmptyFleet(1);
        var cabOne = new Cab("evan", 1, 1.00, 1.00);
        var cabTwo = new Cab("dan", 1, 1.00, 1.00);
        _fleetRepository.Save(cabOne);
        _fleetRepository.Save(cabTwo);
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Equal(2, _cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs.Count);
    }
    [Fact]
    public void CanRemoveCab()
    {
        EmptyFleet(1);
        var cabOne = new Cab("evan", 1, 1.00, 1.00);
        _fleetRepository.Save(cabOne);
        _fleetRepository.Remove(1);
        Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
        Assert.Empty(_cabContext.Fleet
            .Include(x => x.FleetOfCabs)
            .FirstOrDefault()!.FleetOfCabs);
    }
    
    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.Fleet.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Fleet.Add(new Fleet() { Id = 1 });
        _cabContext.SaveChanges();
    }

    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu.Menu() { Id = 1 });
        _cabContext.SaveChanges();
    }

    private void EmptyFleet(int fleetId)
    {
        var fleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
            .FirstOrDefault(x => x.Id == fleetId);
        _cabContext.CabDrivers.RemoveRange(fleet!.FleetOfCabs.ToList());
        _cabContext.SaveChanges();
    }
}