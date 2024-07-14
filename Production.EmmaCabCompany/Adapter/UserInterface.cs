using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;

namespace Production.EmmaCabCompany;

public class UserInterface
{
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private readonly ICabCompanyReader _cabCompanyReader;

    public UserInterface(ICabCompanyPrinter cabCompanyPrinter, ICabCompanyReader cabCompanyReader)
    {
        _cabCompanyPrinter = cabCompanyPrinter;
        _cabCompanyReader = cabCompanyReader;
    }

    public void Run()
    {
        int selection;
        var dispatch = new RadioFleet();
        var dispatchController = new DispatchController(dispatch);
        do
        {
            _cabCompanyPrinter.WriteLine("Please choose a selection from the list: ");
            // also in the command pattern
            // command -> menu option string, -> execute with parameters
            _cabCompanyPrinter.WriteLine("0. Exit");
            _cabCompanyPrinter.WriteLine("1. (Incoming Radio) Add New Cab Driver");
            _cabCompanyPrinter.WriteLine("2. (Incoming Radio) Remove Cab Driver"); // TODO: weird tricks here could cause bugs
            _cabCompanyPrinter.WriteLine("3. (Outgoing Radio) Send Cab Driver Ride Request");
            _cabCompanyPrinter.WriteLine("4. (Incoming Radio) Cab Notifies Passenger Picked Up");
            _cabCompanyPrinter.WriteLine("5. (Incoming Radio) Cab Notifies Passenger Dropped Off");
            _cabCompanyPrinter.WriteLine("6. (Incoming Call) Cancel Cab Driver Fare");
            _cabCompanyPrinter.WriteLine("7. (Incoming Call) Customer Request Ride");
            var lineEntered = _cabCompanyReader.ReadLine();

            _cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
            var isChosen = Int32.TryParse(lineEntered, out selection);
            if (!isChosen)
            {
                continue;
            }

            switch (selection)
            {
                // command pattern
                case 1:
                    var addCabCommand = dispatchController.AddCab();
                    _cabCompanyPrinter.WriteLine(addCabCommand);
                    break;
                case 2:
                    var removeCabCommand = dispatchController.RemoveCab();
                    _cabCompanyPrinter.WriteLine(removeCabCommand);
                    break;
                case 3:
                    var sendCabRequestCommand = dispatchController
                        .SendCabRequest();
                    sendCabRequestCommand.ForEach(x => _cabCompanyPrinter.WriteLine(x));
                    break;
                case 4:
                    var cabNotifiesOfPickup = dispatchController.CabNotifiesPickedUp();
                    _cabCompanyPrinter.WriteLine(cabNotifiesOfPickup);
                    break;
                case 5:
                    var customersPickedUpOutput = dispatchController.CabNotifiesDroppedOff();
                    customersPickedUpOutput.ForEach(x => _cabCompanyPrinter.WriteLine(x));
                    break;
                case 6:
                    var customerCancelledOutput = dispatchController.CustomerCancelledCabRide();
                    customerCancelledOutput.ForEach(x => _cabCompanyPrinter.WriteLine(x));
                    break;
                case 7:
                    var cabCalledOutput = dispatchController.CustomerCabCall();
                    _cabCompanyPrinter.WriteLine(cabCalledOutput);
                    break;
            }
        } while (selection != 0);
    }
}