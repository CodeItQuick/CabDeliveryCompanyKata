using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;

namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class UserInterface
{
    private readonly ICabCompanyPrinter cabCompanyPrinter;
    private readonly ICabCompanyReader cabCompanyReader;
    private MenuController _menuController;
    private CabContext _cabContext;
    private DispatchController _dispatchController;

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
        EnsureFleetExistsForSingleUser();
        EnsureMenuExistsForSingleUser();
        _menuController = new MenuController(new MenuRepository(_cabContext));
        _dispatchController = new DispatchController(new CustomerListRepository(_cabContext), new FleetRepository(_cabContext), new MenuRepository(_cabContext));

    }
    
    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.Fleet.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Fleet.Add(new Fleet() { Id = 1 });
        _cabContext.SaveChanges();
    }

    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Menu() { Id = 1 });
        _cabContext.SaveChanges();
    }

    public void Run()
    {
        int selection;
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

            var output = ExecuteCommand(selection, _dispatchController, paramList.ToArray());
            output.ForEach(cabCompanyPrinter.WriteLine);
        } while (selection != 0);
    }

    private List<string?> RequestParamList(int selection)
    {
        List<string?> paramList = [];
        if (selection == 7 && _menuController.ContainsOption(7))
        {
            paramList.Add(ExtractParam($"Enter customer name: "));
            Console.WriteLine("Location List");
            Console.WriteLine("1 Fulton Drive");
            Console.WriteLine("2 Fulton Drive");
            Console.WriteLine("Bowling Alley");
            Console.WriteLine("Walmart");
            Console.WriteLine("Summerside");
            paramList.Add(ExtractParam($"Enter start location: "));
            paramList.Add(ExtractParam($"Enter end location: "));
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
        var menu = _menuController.DisplayMenu();
        menu.ForEach(Console.WriteLine);
    }

    private List<string> ExecuteCommand(int selection, DispatchController dispatchController,
        params string?[] commandParams)
    {
        if (!_menuController.ContainsOption(selection))
        {
            return ["This is not a valid option."];
        }
        return selection switch
        {
            1 => [dispatchController.AddCab()],
            2 => [dispatchController.RemoveCab()],
            3 => dispatchController.SendCabRequest(),
            4 => [dispatchController.CabNotifiesPickedUp()],
            5 => dispatchController.CabNotifiesDroppedOff(),
            6 => dispatchController.CustomerCancelledCabRide(),
            7 => [dispatchController.CustomerCabCall(commandParams[0], commandParams[1], commandParams[2])],
            _ => []
        };
    }
}