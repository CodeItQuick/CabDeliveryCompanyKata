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
        const int randomStartNumber = 10;
        int selection = randomStartNumber;
        bool isChosen = true;
        var customersServed = new List<Customer>();
        List<Customer> customersCalls = new List<Customer>();
        var dispatch = new Dispatch(_cabCompanyPrinter);
        int currentRideRequestServing = 0;
        int currentAssignedCabServing = 0;
        while (selection != 0 && isChosen)
        {
            _cabCompanyPrinter.WriteLine("Please choose a selection from the list: ");
            // also in the command pattern
            // command -> menu option string, -> execute with parameters
            _cabCompanyPrinter.WriteLine("0. Exit");
            _cabCompanyPrinter.WriteLine("1. Add New Cab Driver");
            _cabCompanyPrinter.WriteLine("2. Remove Cab Driver");
            _cabCompanyPrinter.WriteLine("3. Send Cab Driver Ride Request");
            _cabCompanyPrinter.WriteLine("4. Assign Cab Driver to Ride");
            _cabCompanyPrinter.WriteLine("5. Cancel Cab Driver Fare");
            _cabCompanyPrinter.WriteLine("6. (External) Customer Request Ride");
            var lineEntered = _cabCompanyReader.ReadLine();
        
            _cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
            isChosen = Int32.TryParse(lineEntered, out selection);
            if (isChosen)
            {
                // command pattern
                if (selection == 1)
                {
                    dispatch.AddCab(new Cabs("Evan's Cab", _cabCompanyPrinter, 20));
                }
                if (selection == 2)
                {
                }
                if (selection == 3)
                {
                    if (dispatch.NoCabsInFleet())
                    {
                        _cabCompanyPrinter.WriteLine("There are currently no cabs in the fleet.");
                    }
                    else if (customersCalls.Any())
                    {
                        var customer = customersCalls.Skip(0).FirstOrDefault();
                        dispatch.RideRequest(customer);
                        customersCalls.RemoveAt(0);
                    }
                    else
                    {
                        _cabCompanyPrinter.WriteLine("There are currently no customer's waiting for cabs.");
                    }
                }
                if (selection == 4)
                {
                    if (customersCalls.Count == 0 && customersServed.Count == 0)
                    {
                        _cabCompanyPrinter.WriteLine("There are currently no customer calls in for cabs.");
                    }
                    if (customersCalls.Count == customersServed.Count)
                    {
                        _cabCompanyPrinter.WriteLine("There are currently no customer's assigned to cabs.");
                    }
                    else
                    {
                        var customer = customersServed.Skip(0).First();
                        dispatch.CallCab(customer);
                        customersServed.RemoveAt(0);
                    }
                }
                if (selection == 5)
                {
                }
                if (selection == 6)
                {
                    var customerName = customerNames[numCustomersServed];
                    numCustomersServed++;
                    var customer = new Customer(customerName, "1 Fulton Drive", "1 Destination Lane");
                    customersServed.Add(customer);
                    customersCalls.Add(customer);
                }
            }
        }
    }
}