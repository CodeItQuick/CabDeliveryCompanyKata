using Production.EmmaCabCompany.Commands;

namespace Production.EmmaCabCompany;

public class UserInterface
{
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private readonly ICabCompanyReader _cabCompanyReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileReader _fileReader;

    public UserInterface(
        ICabCompanyPrinter cabCompanyPrinter, ICabCompanyReader cabCompanyReader,
        IFileWriter fileWriter, IFileReader fileReader)
    {
        _cabCompanyPrinter = cabCompanyPrinter;
        _cabCompanyReader = cabCompanyReader;
        _fileWriter = fileWriter;
        _fileReader = fileReader;
    }

    public void Run()
    {
        int numCustomersServed = 0;
        List<string> customerNames = new List<string>()
        {
            "Emma",
            "Lisa",
            "Dan",
            "Evan",
            "Darrell",
            "Diane",
            "Bob",
            "Arlo"
        };
        int selection;
        List<Customer> customersCallInProgress = new List<Customer>();
        List<Customer> customersAwaitingPickup = new List<Customer>();
        List<Customer> customersPickedUp = new List<Customer>();
        var dispatch = new Dispatch();
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
            _cabCompanyPrinter.WriteLine("5. (Incoming Radio) All Cab Notifies Passenger Dropped Off");
            _cabCompanyPrinter.WriteLine("6. (Incoming Call) Cancel Cab Driver Fare");
            _cabCompanyPrinter.WriteLine("7. (Incoming Call) Customer Request Ride");
            var lineEntered = _cabCompanyReader.ReadLine();

            _cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
            var isChosen = Int32.TryParse(lineEntered, out selection);
            if (!isChosen)
            {
                continue;
            }
            // command pattern
            if (selection == 1)
            {
                AddCabCommand.Select(dispatch, _cabCompanyPrinter);
            }

            if (selection == 2)
            {
                RemoveCabCommand.Select(dispatch, _cabCompanyPrinter);
            }
            if (selection == 3)
            {
                SendCabRequestCommand.Select(dispatch, customersCallInProgress, customersAwaitingPickup, _cabCompanyPrinter);
            }
            if (selection == 4)
            {
                CabNotifiesPickedUpCommand.Select(dispatch, customersAwaitingPickup, customersPickedUp, _cabCompanyPrinter);
            }
            if (selection == 5)
            {
                customersPickedUp = CabNotifiesDroppedOffCommand.Select(customersPickedUp, dispatch, _cabCompanyPrinter);
            }
            if (selection == 6)
            {
                CustomerCancelledCabRideCommand.Select(customersAwaitingPickup, customersPickedUp, _cabCompanyPrinter);
            }
            if (selection == 7)
            {
                numCustomersServed = CustomerCabCallCommand.Select(customerNames, numCustomersServed, customersCallInProgress, _cabCompanyPrinter);
            }
        } while (selection != 0);
    }
}