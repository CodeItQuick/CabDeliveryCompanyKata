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
        var dispatch = new Dispatch(_cabCompanyPrinter);
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
                selection = 10;
                continue;
            }
            // command pattern
            if (selection == 1)
            {
                dispatch.AddCab(new Cab("Evan's Cab", _cabCompanyPrinter, 20));
            }

            if (selection == 2)
            {
                if (!dispatch.NoCabsInFleet())
                {
                    dispatch.RemoveCab();
                }
            }
            if (selection == 3)
            {
                if (dispatch.NoCabsInFleet())
                {
                    _cabCompanyPrinter.WriteLine("There are currently no cabs in the fleet.");
                }
                else if (customersCallInProgress.Any())
                {
                    var customer = customersCallInProgress.Skip(0).First();
                    dispatch.RideRequest(customer);
                    customersAwaitingPickup.Add(customer);
                    customersCallInProgress.RemoveAt(0);
                    _cabCompanyPrinter.WriteLine("Cab assigned to customer.");
                }
                else
                {
                    _cabCompanyPrinter.WriteLine("There are currently no customer's waiting for cabs.");
                }
            }
            if (selection == 4)
            {
                if (dispatch.NoCabsInFleet())
                {
                    _cabCompanyPrinter.WriteLine("There are currently no cabs in the fleet.");
                }
                else if (customersAwaitingPickup.Count == 0)
                {
                    _cabCompanyPrinter.WriteLine("There are currently no customer's assigned to cabs.");
                }
                else
                {
                    var customer = customersAwaitingPickup.FirstOrDefault();
                    dispatch.PickupCustomer(customer);
                    customersPickedUp.Add(customer);
                    customersAwaitingPickup.RemoveAt(0);
                }
            }
            if (selection == 5)
            {
                if (customersPickedUp.Count == 0)
                {
                    _cabCompanyPrinter.WriteLine("There are currently no customer's assigned to cabs.");
                }
                else
                {
                    dispatch.DropOffCustomers();
                    customersPickedUp = new List<Customer>();
                }
            }
            if (selection == 6)
            {
                if (customersAwaitingPickup.Any())
                {
                    customersAwaitingPickup.RemoveAt(customersAwaitingPickup.Count - 1);
                    _cabCompanyPrinter.WriteLine("Customer cancelled request before cab assigned.");
                }

                if (customersPickedUp.Any())
                {
                    customersPickedUp.RemoveAt(customersPickedUp.Count - 1);
                    _cabCompanyPrinter.WriteLine("Customer cancelled request before cab got there.");
                }
            }
            if (selection == 7)
            {
                var customerName = customerNames[numCustomersServed];
                numCustomersServed++;
                var customer = new Customer(customerName, "1 Fulton Drive", "1 Destination Lane");
                customersCallInProgress.Add(customer);
                _cabCompanyPrinter.WriteLine($"Received customer ride request from {customerName}");
            }
        } while (selection != 0);
    }
}