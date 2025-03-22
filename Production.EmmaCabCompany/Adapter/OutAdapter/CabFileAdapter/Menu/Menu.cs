using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;

[PrimaryKey("Id")]
[Table("Menu")]
public class Menu : IdentityClass
{
    public override int Id { get; set; }
    [ForeignKey("CustomerId")] public List<PatronDto> Customers { get; init; }
    [ForeignKey("CabId")] public List<CabDriver> Cabs { get; init; }

    public Menu()
    {
        Customers = new List<PatronDto>();
        Cabs = new List<CabDriver>();
    }

    public List<int> MenuOptions()
    {
        List<int> options = [];
        if (Cabs.Count(x => x.IsStatus(CabStatus.Available)) > 
            Customers.Count(x => x.Status != CustomerStatus.Delivered))
        {
            options.AddRange([2, 7]);
        }
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