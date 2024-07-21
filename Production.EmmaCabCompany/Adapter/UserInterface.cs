using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;

namespace Production.EmmaCabCompany;

public class UserInterface(
    ICabCompanyPrinter cabCompanyPrinter, ICabCompanyReader cabCompanyReader,
    IFileHandler writer)
{
    public void Run()
    {
        int selection;
        var dispatch = new DispatcherCoordinator();
        var cabService = new CabService(dispatch, writer);
        var dispatchController = new DispatchController(cabService);
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

    private static void WriteMenu()
    {
        Console.WriteLine("Please choose a selection from the list: ");
        Console.WriteLine("0. Exit");
        Console.WriteLine("1. (Incoming Radio) Add New Cab Driver");
        Console.WriteLine("2. (Incoming Radio) Remove Cab Driver"); // TODO: weird tricks here could cause bugs
        Console.WriteLine("3. (Outgoing Radio) Send Cab Driver Ride Request");
        Console.WriteLine("4. (Incoming Radio) Cab Notifies Passenger Picked Up");
        Console.WriteLine("5. (Incoming Radio) Cab Notifies Passenger Dropped Off");
        Console.WriteLine("6. (Incoming Call) Cancel Cab Driver Fare");
        Console.WriteLine("7. (Incoming Call) Customer Request Ride");
    }

    public static List<string> ExecuteCommand(int selection, DispatchController dispatchController)
    {
        switch (selection)
        {
            case 1:
                return [dispatchController.AddCab()];
            case 2:
                return [dispatchController.RemoveCab()];
            case 3:
                return dispatchController.SendCabRequest();
            case 4:
                return [dispatchController.CabNotifiesPickedUp()];
            case 5:
                return dispatchController.CabNotifiesDroppedOff();
            case 6:
                return dispatchController.CustomerCancelledCabRide();
            case 7:
                return [dispatchController.CustomerCabCall()];
            default:
                return [];
        }
    }
}