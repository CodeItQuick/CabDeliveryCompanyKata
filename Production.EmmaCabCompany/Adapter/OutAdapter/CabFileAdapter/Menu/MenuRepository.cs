using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.Menu;
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
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Menu.Menu() { Id = 1, Customers = new List<CustomerDto>()});
        _cabContext.SaveChanges();
    }
    private void EnsureCustomerListExistsForSingleUser()
    {
        var fleetExists = _cabContext.CustomerList.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.CustomerList.Add(new CustomerListDto() { Id = 1 });
        _cabContext.SaveChanges();
    }

    public Menu.Menu GetById(int customerListId)
    {
        var menu = _cabContext.Menu
            .Include(x => x.Customers)
            .Include(x => x.Cabs)
            .FirstOrDefault(x => x.Id == 1)!;
        return menu;
    }
}