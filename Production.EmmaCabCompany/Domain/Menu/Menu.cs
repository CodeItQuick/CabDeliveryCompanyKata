using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;
using Production.EmmaCabCompany.Domain.Customers;
using Production.EmmaCabCompany.Domain.Fleet;
using CabStatus = Production.EmmaCabCompany.Domain.Fleet.CabStatus;

namespace Production.EmmaCabCompany.Domain.Menu;

public class Menu
{
    public Customer? Customer { get; set; }
    public List<Patron>? Patrons { get; set; }
    public List<Cab>? Cabs { get; set; }
    
    public Menu()
    {
        
    }

    public List<string> MenuOptions()
    {
        List<string> options = [];
        var isCabAvailable = Customer != null && Cabs?.Count(x => x.IsStatus(CabStatus.Available)) > 
            Patrons?.Count(x => x.Status != PatronStatus.Delivered);
        if (isCabAvailable)
        {
            options.AddRange(["2", "7"]);
        }
        if (Customer != null && Patrons != null && Patrons.Any(x => x.Status == PatronStatus.CustomerCallInProgress))
        {
            options.AddRange(["3", "6"]);
        }
        if (Customer != null && Patrons != null && Patrons.Any(x => x.Status == PatronStatus.WaitingPickup))
        {
            options.AddRange(["4"]);
        }
        if (Customer != null && Patrons != null && Patrons.Any(x => x.Status == PatronStatus.Enroute))
        {
            options.AddRange(["5"]);
        }
        return options;
    }
}