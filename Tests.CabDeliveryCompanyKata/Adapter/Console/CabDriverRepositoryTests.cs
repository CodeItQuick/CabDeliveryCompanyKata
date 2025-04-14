using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Customers;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;
using Customer = Production.EmmaCabCompany.Domain.Customers.Customer;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;
public class TestDatabaseFixture
{
    private const string ConnectionString = "Data Source=sqllite_db.db";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    public CabContext CreateContext()
        => new CabContext(
            new DbContextOptionsBuilder<CabContext>()
                .UseInMemoryDatabase(ConnectionString)
                .EnableSensitiveDataLogging()
                .Options);
}
public class CabDriverRepositoryTests : IClassFixture<TestDatabaseFixture>
{
    
    public CabDriverRepositoryTests(TestDatabaseFixture fixture)
        => Fixture = fixture;

    public TestDatabaseFixture Fixture { get; }
    [Fact]
    public void CanAddCab()
    {
        using var cabContext = Fixture.CreateContext();
        var currentCustomers = cabContext.Customers.ToList();
        var currentCabDrivers = cabContext.CabDrivers.ToList();
        cabContext.Customers.RemoveRange(currentCustomers);
        cabContext.CabDrivers.RemoveRange(currentCabDrivers);
        cabContext.Customers.Add(new Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Customer());
        cabContext.SaveChanges();
        using var cabContextTwo = Fixture.CreateContext();
        var cab = new Cab("evan", 1, 1.00, 1.00) { CustomerId = 1 };
        
        var cabDriverRepository = new CabDriverRepository(cabContextTwo);
        cabDriverRepository.Save(cab);
            
        Assert.Single(cabContext.Customers.ToList());
        Assert.Single(cabContext.CabDrivers.ToList());
    }
    [Fact]
    public void CanAddTwoCabs()
    {
        using var cabContext = Fixture.CreateContext();
        var currentCustomers = cabContext.Customers.ToList();
        var currentCabDrivers = cabContext.CabDrivers.ToList();
        cabContext.Customers.RemoveRange(currentCustomers);
        cabContext.CabDrivers.RemoveRange(currentCabDrivers);
        cabContext.Customers.Add(new Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Customer());
        cabContext.SaveChanges();
        var cab = new Cab("evan", 1, 1.00, 1.00) { CustomerId = 1 };
        var cabTwo = new Cab("dan", 1, 1.00, 1.00) { CustomerId = 1 };
        var cabDriverRepository = new CabDriverRepository(cabContext);
        cabDriverRepository.Save(cab);
        var cabDriverRepositoryThree = new CabDriverRepository(cabContext);
        cabContext.Attach(cab);
        cabContext.Entry(cab).State = EntityState.Unchanged;
        cabContext.SaveChanges();
        
        cabDriverRepositoryThree.Save(cabTwo);
            
        Assert.Single(cabContext.Customers.ToList());
        Assert.Single(cabContext.CabDrivers.ToList());
    }
    // [Fact]
    // public void CanRemoveCab()
    // {
    //     var cabOne = new CabDriver("evan", 1, 1.00, 1.00);
    //     var fleet = new Fleet();
    //     _cabDriverRepository.Save(fleet);
    //     fleet.Cabs.Add(cabOne);
    //     _cabDriverRepository.Save(fleet);
    //     
    //     _cabDriverRepository.Remove(1);
    //     
    //     Assert.Equal(1, _cabContext.Fleet.FirstOrDefault()!.Id);
    //     Assert.Empty(_cabContext.Fleet
    //         .Include(x => x.Cabs)
    //         .FirstOrDefault()!.Cabs);
    // }

}