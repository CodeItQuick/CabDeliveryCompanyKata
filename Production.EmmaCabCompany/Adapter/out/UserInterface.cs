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
        var cabService = new CabService(dispatch, new CabFileRepository(writer));
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

            var paramList = RequestParamList(selection);

            var output = ExecuteCommand(selection, dispatchController, paramList); 
                output.ForEach(cabCompanyPrinter.WriteLine);
        } while (selection != 0);
    }

    private string RequestParamList(int selection)
    {
        string? paramList = "";
        if (selection == 7)
        {
            while (paramList == null || string.IsNullOrWhiteSpace(paramList))
            {
                cabCompanyPrinter.WriteLine($"Enter customer name: ");
                paramList = cabCompanyReader.ReadLine();
            }
        }

        return paramList;
    }

    private void WriteMenu()
    {
        var menu = _menuController.DisplayMenu();
        menu.ForEach(Console.WriteLine);
    }

    private static List<string> ExecuteCommand(int selection, DispatchController dispatchController, params string[] commandParams)
    {
        return selection switch
        {
            1 => [dispatchController.AddCab()],
            2 => [dispatchController.RemoveCab()],
            3 => dispatchController.SendCabRequest(),
            4 => [dispatchController.CabNotifiesPickedUp()],
            5 => dispatchController.CabNotifiesDroppedOff(),
            6 => dispatchController.CustomerCancelledCabRide(),
            7 => [dispatchController.CustomerCabCall(commandParams[0])],
            _ => []
        };
    }
}