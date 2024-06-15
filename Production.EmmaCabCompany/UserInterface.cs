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
        const int randomStartNumber = 10;
        int selection = randomStartNumber;
        bool isChosen = true;
        var customers = new List<Customer>();
        var dispatch = new Dispatch(_cabCompanyPrinter);
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
                    dispatch.RideRequest(customers.First());
                }
                if (selection == 4)
                {
                    dispatch.CallCab(customers.First());
                    customers.RemoveAt(0);
                }
                if (selection == 5)
                {
                }
                if (selection == 6)
                {
                    customers.Add(new Customer("Emma", "1 Fulton Drive", "1 Destination Lane"));
                }
            }
        }
    }
}