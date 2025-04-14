using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Domain.Fleet;

public class Fleet
{
    public List<Cab> Cabs { get; set; } = new();
}
