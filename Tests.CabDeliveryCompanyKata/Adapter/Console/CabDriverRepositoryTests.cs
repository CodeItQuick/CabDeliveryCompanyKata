using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;


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
    private DbContextOptions<CabContext> _dbContextOptions;
    private DbConnectionStringBuilder _dbConnection;
    private SqliteConnection _sqliteConnection;

    public CabDriverRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _dbConnection = new DbConnectionStringBuilder
        {
            ConnectionString = "DataSource=sqlite_db.db"
        };
        _sqliteConnection = new SqliteConnection(_dbConnection.ConnectionString);
        _sqliteConnection.Open();
        _dbContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite(_sqliteConnection)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .Options;
        _cabContext = new CabContext(_dbContextOptions);
        _cabContext.Database.Migrate();
        var customerList = _cabContext.Customers.ToList();
        _cabContext.Customers.RemoveRange(customerList);
        _cabContext.CabDrivers.RemoveRange(_cabContext.CabDrivers.ToList());
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    [Fact]
    public void CanAddCab()
    {
        var customer = new Customer();
        _cabContext.Customers.Add(customer);
        _cabContext.SaveChanges();
        var cab = new Cab("evan", 1, 1.00, 1.00)
        {
            CustomerId = customer.Id, Id = null
        };

        var cabDriverRepository = new CabDriverRepository(_cabContext);
        cabDriverRepository.Save(cab);

        Assert.Single(_cabContext.Customers.ToList());
        Assert.Single(_cabContext.CabDrivers.ToList());
        Assert.Equal(customer.Id, _cabContext.CabDrivers.ToList().FirstOrDefault()?.Customer.Id);
    }

    [Fact]
    public void CanAddTwoCabs()
    {
        var customer = new Customer();
        _cabContext.Customers.Add(customer);
        _cabContext.SaveChanges();
        
        var cab = new Cab("evan", 1, 1.00, 1.00) { Id = null, CustomerId = customer.Id };
        var cabTwo = new Cab("dan", 1, 1.00, 1.00) { CustomerId = customer.Id };
        var cabDriverRepository = new CabDriverRepository(_cabContext);
        cabDriverRepository.Save(cab);
        cabDriverRepository.Dispose();

        var cabContextTwo = new CabContext(_dbContextOptions);
        var cabDriverRepositoryTwo = new CabDriverRepository(cabContextTwo);
        cabDriverRepositoryTwo.Save(cabTwo);
        
        Assert.Single(cabContextTwo.Customers.ToList());
        Assert.Equal(2, cabContextTwo.CabDrivers.Count());
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
            .Where(x => x.Customer.Id == customer.Id)
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