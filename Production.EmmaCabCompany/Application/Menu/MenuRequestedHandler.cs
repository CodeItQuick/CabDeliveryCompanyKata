using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Domain.Menu;

namespace Production.EmmaCabCompany.Application.Menu;

public class MenuRequestedHandler : IMenuRequestedHandler
{
    private readonly IMenuRepository _menuRepository;

    public MenuRequestedHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public MenuConfigurationDto Handle(MenuRequested menuRequested)
    {
        var menuConfigurationDto = new MenuConfigurationDto();
        var menu = _menuRepository.GetById(menuRequested._id);
        if (menu == null)
        {
            return menuConfigurationDto;
        }
        // menuConfigurationDto.MenuOptions = ["0", "1"];
        menuConfigurationDto.MenuOptions.AddRange(menu.MenuOptions());
        return menuConfigurationDto;
    }
}

public interface IMenuRequestedHandler
{
    public MenuConfigurationDto Handle(MenuRequested menuRequested);
}