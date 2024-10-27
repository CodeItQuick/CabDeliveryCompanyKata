using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class MenuRequestedHandler
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
        menuConfigurationDto.MenuOptions.AddRange(menu.MenuOptions());
        return menuConfigurationDto;
    }
}