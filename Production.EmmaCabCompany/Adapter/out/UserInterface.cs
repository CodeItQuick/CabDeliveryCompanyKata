using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;
using Tests.CabDeliveryCompanyKata;

namespace Production.EmmaCabCompany.Adapter.@out;

public class UserInterface(
    ICabCompanyPrinter cabCompanyPrinter, ICabCompanyReader cabCompanyReader,
    IFileHandler writer)
{
    private MenuController _menuController;

    public void Run()
    {
        int selection;
        var dispatch = new DispatcherCoordinator();
        var cabService = new CabService(dispatch, writer);
        _menuController = new MenuController(new MenuService(dispatch));
        var dispatchController = new DispatchController(cabService, new MenuService(dispatch));
        do
        {
            WriteMenu();
            var lineEntered = cabCompanyReader.ReadLine();

            cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
            var isChosen = Int32.TryParse(lineEntered, out selection);
            if (!isChosen)
            {
                continue;
            }

            var output = ExecuteCommand(selection, dispatchController); 
            output.ForEach(cabCompanyPrinter.WriteLine);
        } while (selection != 0);
    }

    private void WriteMenu()
    {
        var menu = _menuController.DisplayMenu();
        menu.ForEach(Console.WriteLine);
    }

    private static List<string> ExecuteCommand(int selection, DispatchController dispatchController)
    {
        return selection switch
        {
            1 => [dispatchController.AddCab()],
            2 => [dispatchController.RemoveCab()],
            3 => dispatchController.SendCabRequest(),
            4 => [dispatchController.CabNotifiesPickedUp()],
            5 => dispatchController.CabNotifiesDroppedOff(),
            6 => dispatchController.CustomerCancelledCabRide(),
            7 => [dispatchController.CustomerCabCall()],
            _ => []
        };
    }
}