using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CabContext : DbContext
{
    public DbSet<Fleet.Fleet> Fleet { get; set; }
    public DbSet<Cab> Cabs { get; set; }
    public DbSet<CustomerDto> Customers { get; set; }
    public DbSet<CustomerListDto> CustomerList { get; set; }
    public DbSet<Menu.Menu> Menu { get; set; }

    public CabContext(DbContextOptions<CabContext> options) : base(options) { }
}
