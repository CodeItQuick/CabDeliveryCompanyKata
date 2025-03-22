using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class UnitOfWork : IDisposable
{
    private CabContext context;
    public CustomerListRepository CustomerListRepository;
    public FleetRepository FleetRepository;
    public MenuRepository MenuRepository;

    public UnitOfWork(CabContext context)
    {
        this.context = context;
        CustomerListRepository = new CustomerListRepository(this.context);
        FleetRepository = new FleetRepository(this.context);
        MenuRepository = new MenuRepository(this.context);
    }
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}