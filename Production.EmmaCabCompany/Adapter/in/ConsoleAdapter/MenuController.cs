using Tests.CabDeliveryCompanyKata;

namespace Production.EmmaCabCompany.Adapter.@out;

public class MenuController
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService)
    {
        _menuService = menuService;
    }

    public List<string> DisplayMenu()
    {
        var displayMenu = _menuService.DisplayMenu();
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
            .Where((_, idx) => displayMenu.Contains(idx));
        menu.AddRange(selectAdditionalOptions);
        return menu;
    }
}