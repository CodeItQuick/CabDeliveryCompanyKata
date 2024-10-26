using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class MenuService(DispatcherCoordinator dispatcherCoordinator)
{
    public List<int> DisplayMenu()
    {
        var menuState = dispatcherCoordinator.MenuState();
        List<int> defaultMenuItems = [0, 1, 2, 7];
        defaultMenuItems.AddRange(menuState);
        defaultMenuItems.Sort();
        return defaultMenuItems;
    }

    public bool IsValidMenuOption(int option)
    {
        var menuState = dispatcherCoordinator.MenuState();
        return menuState.Contains(option);
    }
}