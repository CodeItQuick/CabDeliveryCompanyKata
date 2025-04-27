using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CabContext : DbContext
{
    public DbSet<CabDriver> CabDrivers { get; set; }
    public DbSet<PatronDto> Patrons { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public CabContext(DbContextOptions<CabContext> options) : base(options) { }
    public CabContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CabContext>();
        optionsBuilder.UseSqlite("Data Source=cab_database.db");

        return new CabContext(optionsBuilder.Options);
    }
}
