using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CabContext : DbContext
{
    public DbSet<Fleet.Fleet> Fleet { get; set; }
    public DbSet<CabDriver> CabDrivers { get; set; }
    public DbSet<PatronDto> Patron { get; set; }
    public DbSet<FleetCoordinator> FleetCoordinator { get; set; }
    public DbSet<Menu.Menu> Menu { get; set; }

    public CabContext(DbContextOptions<CabContext> options) : base(options) { }
}
