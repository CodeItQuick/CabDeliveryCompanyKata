using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Application.Menu;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;

public class MenuRepository : IMenuRepository, IDisposable
{
    private readonly CabContext _cabContext;

    public MenuRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
        EnsureMenuExistsForSingleUser();
        EnsureCustomerListExistsForSingleUser();
    }

    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new CabFileAdapter.Menu.Menu() { Id = 1, Customers = new List<PatronDto>()});
        _cabContext.SaveChanges();
    }
    private void EnsureCustomerListExistsForSingleUser()
    {
        var fleetExists = _cabContext.FleetCoordinator.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.FleetCoordinator.Add(new FleetCoordinator() { Id = 1 });
        _cabContext.SaveChanges();
    }

    public Menu GetById(int customerListId)
    {
        var menu = _cabContext.Menu
            .Include(x => x.Customers)
            .Include(x => x.Cabs)
            .FirstOrDefault(x => x.Id == 1)!;
        return menu;
    }
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _cabContext.Dispose();
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