using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Customers;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Patrons;
using Production.EmmaCabCompany.Application.Menu;
using Production.EmmaCabCompany.Domain.Menu;

namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class MenuController
{
    private readonly MenuRequestedHandler _menuRequested;

    public MenuController(CabContext cabContext)
    {
        _menuRequested = new MenuRequestedHandler(
            new PatronsRepository(cabContext),
            new CustomerRepository(cabContext),
            new CabDriversRepository(cabContext));
    }

    public List<string> DisplayMenu()
    {
        var menuOptions = _menuRequested.Handle(new MenuRequested(1));
        var menu = new List<string>()
        {
            "Please choose a selection from the list: ",
        };
        var otherMenuOptions = new List<string>()
        {
            "0. Exit",
            "1. (Incoming Radio) Add New Cab Driver",
            "2. (Incoming Radio) Remove Cab Driver",
            "3. (Outgoing Radio) Send Cab Driver Ride Request",
            "4. (Incoming Radio) Cab Notifies Passenger Picked Up",
            "5. (Incoming Radio) Cab Notifies Passenger Dropped Off",
            "6. (Incoming Call) Cancel Cab Driver Fare",
            "7. (Incoming Call) Customer Request Ride",
            "8. Register New User",
            "9. Login Existing User"
        };
        var selectAdditionalOptions = otherMenuOptions
            .Where((_, idx) => menuOptions.MenuOptions.Contains(idx.ToString()));
        menu.AddRange(selectAdditionalOptions);
        return menu;
    }

    public bool ContainsOption(string selection)
    {
        var menuConfiguration = _menuRequested.Handle(new MenuRequested(1));
        return menuConfiguration.MenuOptions.Contains(selection);
    }
}