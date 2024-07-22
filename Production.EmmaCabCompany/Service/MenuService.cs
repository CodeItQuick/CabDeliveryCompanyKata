using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class MenuService    
{
    private readonly DispatcherCoordinator _dispatcherCoordinator;

    public MenuService(DispatcherCoordinator dispatcherCoordinator)
    {
        _dispatcherCoordinator = dispatcherCoordinator;
    }

    public List<int> DisplayMenu()
    {
        var menuState = _dispatcherCoordinator.MenuState();
        List<int> defaultMenuItems = [0, 1, 2, 7];
        defaultMenuItems.AddRange(menuState);
        defaultMenuItems.Sort();
        return defaultMenuItems;
    }
}