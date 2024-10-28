using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class Menu
{
    public int Id { get; set; }
    public virtual List<Customer> Customers { get; init; }

    public List<int> MenuOptions()
    {
        List<int> options = [];
        if (Customers.Any(x => x.Status == CustomerStatus.CustomerCallInProgress))
        {
            options.AddRange([3, 6]);
        }
        if (Customers.Any(x => x.Status == CustomerStatus.WaitingPickup))
        {
            options.AddRange([4]);
        }
        if (Customers.Any(x => x.Status == CustomerStatus.Enroute))
        {
            options.AddRange([5]);
        }
        return options;
    }
}