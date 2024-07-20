using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;

namespace Production.EmmaCabCompany;

public class UserInterface(ICabCompanyPrinter cabCompanyPrinter, ICabCompanyReader cabCompanyReader)
{
    public void Run()
    {
        int selection;
        var dispatch = new DispatcherCoordinator();
        var dispatchController = new DispatchController(new CabService(dispatch));
        do
        {
            cabCompanyPrinter.WriteLine("Please choose a selection from the list: ");
            // also in the command pattern
            // command -> menu option string, -> execute with parameters
            cabCompanyPrinter.WriteLine("0. Exit");
            cabCompanyPrinter.WriteLine("1. (Incoming Radio) Add New Cab Driver");
            cabCompanyPrinter.WriteLine("2. (Incoming Radio) Remove Cab Driver"); // TODO: weird tricks here could cause bugs
            cabCompanyPrinter.WriteLine("3. (Outgoing Radio) Send Cab Driver Ride Request");
            cabCompanyPrinter.WriteLine("4. (Incoming Radio) Cab Notifies Passenger Picked Up");
            cabCompanyPrinter.WriteLine("5. (Incoming Radio) Cab Notifies Passenger Dropped Off");
            cabCompanyPrinter.WriteLine("6. (Incoming Call) Cancel Cab Driver Fare");
            cabCompanyPrinter.WriteLine("7. (Incoming Call) Customer Request Ride");
            var lineEntered = cabCompanyReader.ReadLine();

            cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
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
                    cabCompanyPrinter.WriteLine(addCabCommand);
                    break;
                case 2:
                    var removeCabCommand = dispatchController.RemoveCab();
                    cabCompanyPrinter.WriteLine(removeCabCommand);
                    break;
                case 3:
                    var sendCabRequestCommand = dispatchController
                        .SendCabRequest();
                    sendCabRequestCommand.ForEach(x => cabCompanyPrinter.WriteLine(x));
                    break;
                case 4:
                    var cabNotifiesOfPickup = dispatchController.CabNotifiesPickedUp();
                    cabCompanyPrinter.WriteLine(cabNotifiesOfPickup);
                    break;
                case 5:
                    var customersPickedUpOutput = dispatchController.CabNotifiesDroppedOff();
                    customersPickedUpOutput.ForEach(x => cabCompanyPrinter.WriteLine(x));
                    break;
                case 6:
                    var customerCancelledOutput = dispatchController.CustomerCancelledCabRide();
                    customerCancelledOutput.ForEach(x => cabCompanyPrinter.WriteLine(x));
                    break;
                case 7:
                    var cabCalledOutput = dispatchController.CustomerCabCall();
                    cabCompanyPrinter.WriteLine(cabCalledOutput);
                    break;
            }
        } while (selection != 0);
    }
}