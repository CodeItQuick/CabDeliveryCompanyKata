using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Application;

public class MenuService(DispatcherCoordinator dispatcherCoordinator, CabFileRepository cabFileRepository)
{
    public List<int> DisplayMenu()
    {
        var customerDirectory = cabFileRepository.RetrieveCustomerDirectory();
        dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        var loadedFleetState = cabFileRepository.RetrieveFleet();
        dispatcherCoordinator.RebuildCabList(loadedFleetState);
        
        var menuState = dispatcherCoordinator.MenuState();
        List<int> defaultMenuItems = [0, 1, 2, 7];
        defaultMenuItems.AddRange(menuState);
        defaultMenuItems.Sort();
        return defaultMenuItems;
    }

    public bool IsValidMenuOption(int option)
    {
        var customerDirectory = cabFileRepository.RetrieveCustomerDirectory();
        dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        var loadedFleetState = cabFileRepository.RetrieveFleet();
        dispatcherCoordinator.RebuildCabList(loadedFleetState);
        
        var menuState = dispatcherCoordinator.MenuState();
        return menuState.Contains(option);
    }
}