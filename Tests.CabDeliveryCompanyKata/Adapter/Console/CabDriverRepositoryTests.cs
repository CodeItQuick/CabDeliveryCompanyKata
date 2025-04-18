using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;
using Xunit.Sdk;


namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class TestDatabaseFixture
{
    private const string ConnectionString = "Data Source=:memory:";

    public readonly SqlConnection sqlConnection;

    public TestDatabaseFixture()
    {
        sqlConnection = new SqlConnection(ConnectionString);
    }
}

public class CabDriverRepositoryTests : IClassFixture<TestDatabaseFixture>, IDisposable
{
    private readonly TestDatabaseFixture _fixture;
    private CabContext _cabContext;

    public CabDriverRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        var dbContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseInMemoryDatabase(_fixture.sqlConnection.ToString())
            .Options;
        _cabContext = new CabContext(dbContextOptions);
        _cabContext.Customers.RemoveRange(_cabContext.Customers.ToList());
        _cabContext.CabDrivers.RemoveRange(_cabContext.CabDrivers.ToList());
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    [Fact]
    public void CanAddCab()
    {
        var customer = new Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter
            .Customer();
        _cabContext.Customers.Add(customer);
        _cabContext.SaveChanges();
        var cab = new Cab("evan", 1, 1.00, 1.00)
        {
            CustomerId = customer.Id
        };

        var cabDriverRepository = new CabDriverRepository(_cabContext);
        cabDriverRepository.Save(cab);

        Assert.Single(_cabContext.Customers.ToList());
        Assert.Single(_cabContext.CabDrivers.ToList());
        Assert.Equal(customer.Id, _cabContext.CabDrivers.ToList().FirstOrDefault()?.CustomerId);
    }

    [Fact]
    public void CanAddTwoCabs()
    {
        var customer = new Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Customer();
        _cabContext.Customers.Add(customer);
        _cabContext.SaveChanges();
        var cab = new Cab("evan", 1, 1.00, 1.00) { CustomerId = customer.Id };
        var cabTwo = new Cab("dan", 1, 1.00, 1.00) { CustomerId = customer.Id };
        var cabDriverRepository = new CabDriverRepository(_cabContext);
        cabDriverRepository.Save(cab);
        _cabContext.ChangeTracker.Clear();

        cabDriverRepository.Save(cabTwo);

        Assert.Single(_cabContext.Customers.ToList());
        Assert.Equal(2, _cabContext.CabDrivers.Count());
    }

    // Experimenting with streaming
    [Fact]
    public void CanStreamCabDataToFile()
    {
        var customer = new Customer();
        _cabContext.Customers.Add(customer);
        _cabContext.SaveChanges();
        var cab = new Cab("evan", 1, 1.00, 1.00) { CustomerId = customer.Id };
        var cabTwo = new Cab("dan", 1, 1.00, 1.00) { CustomerId = customer.Id };
        var cabDriverRepository = new CabDriverRepository(_cabContext);
        cabDriverRepository.Save(cab);
        _cabContext.ChangeTracker.Clear();
        cabDriverRepository.Save(cabTwo);
        _cabContext.ChangeTracker.Clear();

        cabDriverRepository.StreamToFile("test.csv", customer.Id)
            .GetAwaiter()
            .GetResult();

        var file = File.ReadAllText("test.csv");

        var cabDrivers = _cabContext.CabDrivers
            .Where(x => x.CustomerId == customer.Id)
            .ToList();
        
        Assert.Equal($"{cabDrivers.First().Id}, {cabDrivers.First()._cabName}, 1, 1\n" +
                     $"{cabDrivers.Skip(1).First().Id}, {cabDrivers.Skip(1).First()._cabName}, 1, 1\n", file);
    }
    [Fact]
    public void CanRemoveCab()
    {var customer = new Customer();
        _cabContext.Customers.Add(customer);
        _cabContext.SaveChanges();
        var cab = new Cab("evan", 1, 1.00, 1.00) { CustomerId = customer.Id };
        var cabTwo = new Cab("dan", 1, 1.00, 1.00) { CustomerId = customer.Id };
        var cabDriverRepository = new CabDriverRepository(_cabContext);
        cabDriverRepository.Save(cab);
        _cabContext.ChangeTracker.Clear();
        cabDriverRepository.Save(cabTwo);
        _cabContext.ChangeTracker.Clear();
        
        cabDriverRepository.Remove(customer.Id);
        _cabContext.ChangeTracker.Clear();
        
        Assert.Equal(1, _cabContext.CabDrivers.Count());
        Assert.Null(_cabContext.CabDrivers
            .ToList().Skip(1).FirstOrDefault());
    }

    public void Dispose()
    {
        File.Delete("sqllite_db.db");
    }
}