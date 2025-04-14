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
}
