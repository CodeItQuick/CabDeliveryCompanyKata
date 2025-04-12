using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Application.Menu;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;

public class MenuRepository : IMenuRepository
{
    private readonly CabContext _cabContext;

    public MenuRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public Menu GetById(Menu entity)
    {
        var menu = _cabContext.Menu
            .Include(x => x.Patrons)
            .Include(x => x.Cabs)
            .FirstOrDefault(x => x.Id == entity.Id);
        return menu;
    }

    public void Save(Menu menu)
    {
        var currentMenuExists = _cabContext.Menu.FirstOrDefault(x => x.Id == menu.Id);
        if (currentMenuExists != null)
        {
            currentMenuExists.Cabs = menu.Cabs;
            currentMenuExists.Patrons = menu.Patrons;
            _cabContext.Menu.Update(currentMenuExists);
        }
        else
        {
            _cabContext.Menu.Add(menu);
        }
        _cabContext.SaveChanges();
    }

    public void Remove(int entityId)
    {
        throw new NotImplementedException();
    }
}