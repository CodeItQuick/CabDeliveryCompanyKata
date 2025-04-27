using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class UserInterface
{
    private readonly ICabCompanyPrinter cabCompanyPrinter;
    private readonly ICabCompanyReader cabCompanyReader;
    private MenuController _menuController;
    private CabContext _cabContext;
    private DispatchController _dispatchController;
    private int? CustomerId { get; set; }

    public UserInterface(
        ICabCompanyPrinter cabCompanyPrinter, ICabCompanyReader cabCompanyReader, string? dbName = null)
    {
        this.cabCompanyPrinter = cabCompanyPrinter;
        this.cabCompanyReader = cabCompanyReader;
        var connectionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            dbName ?? $"production_db-{Guid.NewGuid()}.db");
        if (!File.Exists(connectionFile))
        {
            File.Create(connectionFile);
        }

        var dbContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={connectionFile}");
        _cabContext = new CabContext(dbContextOptions.Options);
        _cabContext.Database.Migrate();
        _menuController = new MenuController(_cabContext);
        _dispatchController = new DispatchController(_cabContext);

    }
    
    public void Run()
    {
        string selection = string.Empty;
        do
        {
            WriteMenu();
            var lineEntered = cabCompanyReader.ReadLine();

            cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
            selection = lineEntered ?? string.Empty;
            if (string.IsNullOrEmpty(selection))
            {
                continue;
            }

            var paramList = RequestParamList(selection);

            var output = ExecuteCommand(selection, _dispatchController, paramList.ToArray());
            if (output.Count == 1 && Int32.TryParse(output.First(), out var loginId))
            {
                CustomerId = loginId;
            }
            output.ForEach(cabCompanyPrinter.WriteLine);
        } while (selection != "0");
    }

    private List<string?> RequestParamList(string selection)
    {
        List<string?> paramList = [];
        switch (selection)
        {
            case "7" when _menuController.ContainsOption("7", CustomerId):
                paramList.Add(ExtractParam($"Enter customer name: "));
                Console.WriteLine("Location List");
                Console.WriteLine("1 Fulton Drive");
                Console.WriteLine("2 Fulton Drive");
                Console.WriteLine("Bowling Alley");
                Console.WriteLine("Walmart");
                Console.WriteLine("Summerside");
                paramList.Add(ExtractParam($"Enter start location: "));
                paramList.Add(ExtractParam($"Enter end location: "));
                break;
            case "9" when _menuController.ContainsOption("9", CustomerId):
                paramList.Add(ExtractParam($"Enter user id: "));
                CustomerId = Convert.ToInt32(paramList[0]);
                break;
        }

        return paramList;
    }

    private string ExtractParam(string prompt)
    {
        string param = "";
        while (string.IsNullOrWhiteSpace(param))
        {
            cabCompanyPrinter.WriteLine(prompt);
            string? customerName = cabCompanyReader.ReadLine();
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                param = customerName;
            }
        }

        return param;
    }
    private void WriteMenu()
    {
        var menu = _menuController.DisplayMenu(CustomerId);
        menu.ForEach(Console.WriteLine);
    }

    private List<string> ExecuteCommand(string selection, DispatchController dispatchController,
        params string?[] commandParams)
    {
        if (!_menuController.ContainsOption(selection, CustomerId) && !(CustomerId != null && selection == "9"))
        {
            return ["This is not a valid option."];
        }
        return selection switch
        {
            "1" => [dispatchController.AddCab(CustomerId)],
            "2" => [dispatchController.RemoveCab(CustomerId)],
            "3" => dispatchController.SendCabRequest(CustomerId),
            "4" => [dispatchController.CabNotifiesPickedUp(CustomerId)],
            "5" => dispatchController.CabNotifiesDroppedOff(CustomerId),
            "6" => dispatchController.CustomerCancelledCabRide(CustomerId),
            "7" => [dispatchController.CustomerCabCall(commandParams[0], commandParams[1], commandParams[2], CustomerId)],
            "8" => [dispatchController.RegisterNewUser()],            
            "9" => [dispatchController.LoginUser(CustomerId)],            
            _ => []
        };
    }
}