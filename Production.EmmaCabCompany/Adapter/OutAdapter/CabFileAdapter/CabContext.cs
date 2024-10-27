using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CabContext : DbContext
{
    public DbSet<Fleet> Fleet { get; set; }
    public DbSet<Cab> Cabs { get; set; }
    public DbSet<CustomerList> CustomerList { get; set; }

    public CabContext(DbContextOptions<CabContext> options) : base(options) { }
}
