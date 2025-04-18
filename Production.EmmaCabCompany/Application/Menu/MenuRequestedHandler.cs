using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.Menu;

namespace Production.EmmaCabCompany.Application.Menu;

public class MenuRequestedHandler : IMenuRequestedHandler
{
    private readonly IPatronsRepository _patronsRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICabDriversRepository _cabDriversRepository;

    public MenuRequestedHandler(
        IPatronsRepository patronsRepository,
        ICustomerRepository customerRepository,
        ICabDriversRepository cabDriversRepository)
    {
        _patronsRepository = patronsRepository;
        _customerRepository = customerRepository;
        _cabDriversRepository = cabDriversRepository;
    }

    public MenuConfigurationDto Handle(MenuRequested menuRequested)
    {
        var customer = _customerRepository.GetById(menuRequested.Id);
        var cabDrivers = _cabDriversRepository.GetById(menuRequested.Id);
        var patrons = _patronsRepository.GetById(menuRequested.Id);
        
        var menu = new Domain.Menu.Menu
        {
            Cabs = cabDrivers.Cabs,
            Patrons = patrons.Patrons,
            Customer = customer
        };
        
        return new MenuConfigurationDto() { MenuOptions = menu.MenuOptions(menuRequested.Id)};
    }
}

public interface IMenuRequestedHandler
{
    public MenuConfigurationDto Handle(MenuRequested menuRequested);
}