using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class MenuRepository : IMenuRepository
{
    private CabContext _cabContext;

    public MenuRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
        EnsureMenuExistsForSingleUser();
        EnsureCustomerListExistsForSingleUser();
    }

    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = Queryable.Any<Menu>(_cabContext.Menu, x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Menu() { Id = 1 });
        _cabContext.SaveChanges();
    }
    private void EnsureCustomerListExistsForSingleUser()
    {
        var fleetExists = _cabContext.CustomerList.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.CustomerList.Add(new CustomerListDto() { Id = 1 });
        _cabContext.SaveChanges();
    }

    public Menu GetById(int customerListId)
    {
        var menu = _cabContext.Menu
            .Include(x => x.Customers)
            .FirstOrDefault(x => x.Id == 1)!;
        return menu;
    }
}