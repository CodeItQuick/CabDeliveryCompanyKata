using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Adapter.@out.CabFileAdapter;

public class CabContext : DbContext
{
    public DbSet<Fleet> Fleet { get; set; }
    
    public CabContext(DbContextOptions<CabContext> options) : base(options) { }
}

public interface IFeetContext
{
}