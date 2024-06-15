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

    public void Run(List<ICabs> cabsList)
    {
        const int randomStartNumber = 10;
        int selection = randomStartNumber;
        bool isChosen = true;
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
                    cabsList.Add(new Cabs("Evan's Cab", _cabCompanyPrinter, 20));
                }
                if (selection == 2)
                {
                    if (cabsList.Any())
                    {
                        
                    }
                }
                if (selection == 3)
                {
                }
                if (selection == 4)
                {
                }
                if (selection == 5)
                {
                    _cabCompanyPrinter.WriteLine("Evan's Cab picked up default customer 1 at start location 1.");
                    _cabCompanyPrinter.WriteLine("Evan's Cab dropped off default customer 1 at end location 1.");
                }
                if (selection == 6)
                {
                    
                }
            }
        }
    }
}