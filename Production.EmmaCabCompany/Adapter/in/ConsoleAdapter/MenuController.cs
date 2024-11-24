using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.Menu;
using Tests.CabDeliveryCompanyKata;
using Tests.CabDeliveryCompanyKata.Adapter.Console;

namespace Production.EmmaCabCompany.Adapter.@out;

public class MenuController
{
    private readonly MenuService _menuService;
    private readonly MenuRequestedHandler _menuRequested;

    public MenuController(MenuService menuService, MenuRepository menuRepository)
    {
        _menuService = menuService;
        _menuRequested = new MenuRequestedHandler(menuRepository);
    }

    public List<string> DisplayMenu()
    {
        // var displayMenu = _menuService.DisplayMenu();
        var menuOptions = _menuRequested.Handle(new MenuRequested(1));
        var menu = new List<string>()
        {
            "Please choose a selection from the list: ",
        };
        var otherMenuOptions = new List<string>()
        {
            "0. Exit",
            "1. (Incoming Radio) Add New Cab Driver",
            "2. (Incoming Radio) Remove Cab Driver",
            "3. (Outgoing Radio) Send Cab Driver Ride Request",
            "4. (Incoming Radio) Cab Notifies Passenger Picked Up",
            "5. (Incoming Radio) Cab Notifies Passenger Dropped Off",
            "6. (Incoming Call) Cancel Cab Driver Fare",
            "7. (Incoming Call) Customer Request Ride"
        };
        var selectAdditionalOptions = otherMenuOptions
            .Where((_, idx) => menuOptions.MenuOptions.Contains(idx));
        menu.AddRange(selectAdditionalOptions);
        return menu;
    }

    public bool ContainsOption(int selection)
    {
        var menuConfiguration = _menuRequested.Handle(new MenuRequested(1));
        return menuConfiguration.MenuOptions.Contains(selection);
    }
}