using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.Menu;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;
using Production.WebCabCompany.Models;
using Tests.CabDeliveryCompanyKata;
using Tests.CabDeliveryCompanyKata.Adapter.Console;

namespace Production.WebCabCompany.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IFleetRepository _fleetRepository;
    private readonly IAddCabCommandHandler _addCabCommandHandler;
    private readonly IRemoveCabCommandHandler _removeCabCommandHandler;
    private readonly ICustomerCabRequestedHandler _customerCabRequestedHandler;
    private readonly ICustomerCancelledCabHandler _customerCancelledCabHandler;
    private readonly ICustomerDeliveredHandler _customerDeliveredHandler;
    private readonly ICustomerEnroutedHandler _customerEnroutedHandler;
    private readonly ICustomerRideRequestedHandler _customerRideRequestedHandler;
    private readonly IMenuRequestedHandler _menuRequestedHandler;
    private readonly CabServiceHandler _cabService;

    public HomeController(ILogger<HomeController> logger, 
        IOptions<FileSettings> fileSettings, 
        IFleetRepository fleetRepository,
        IAddCabCommandHandler addCabCommandHandler,
        IRemoveCabCommandHandler removeCabCommandHandler,
        ICustomerCabRequestedHandler customerCabRequestedHandler,
        ICustomerCancelledCabHandler customerCancelledCabHandler,
        ICustomerDeliveredHandler customerDeliveredHandler,
        ICustomerEnroutedHandler customerEnroutedHandler,
        ICustomerRideRequestedHandler customerRideRequestedHandler,
        IMenuRequestedHandler menuRequestedHandler)
    {
        _logger = logger;
        _fleetRepository = fleetRepository;
        _addCabCommandHandler = addCabCommandHandler;
        _removeCabCommandHandler = removeCabCommandHandler;
        _customerCabRequestedHandler = customerCabRequestedHandler;
        _customerCancelledCabHandler = customerCancelledCabHandler;
        _customerDeliveredHandler = customerDeliveredHandler;
        _customerEnroutedHandler = customerEnroutedHandler;
        _customerRideRequestedHandler = customerRideRequestedHandler;
        _menuRequestedHandler = menuRequestedHandler;
        var fileHandler = new FileHandler(
            fileSettings.Value.CustomerFileNameCsv, 
            fileSettings.Value.CabFileNameCsv);
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabFileRepository = new CabFileRepository(fileHandler);
        _cabService = new CabServiceHandler(dispatcherCoordinator, cabFileRepository);
    }

    // TODO: Not tested
    public IActionResult Index()
    {
        var displayMenu = _menuRequestedHandler.Handle(new MenuRequested(1));
        return View(new CabDisplayModel() { DisplayMenu = displayMenu.MenuOptions });
    }

    public IActionResult AddCabDriver()
    {
        _addCabCommandHandler.Handle(new AddCabCommand("default", 23.23, 32.32));
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult RemoveCabDriver()
    {
        _removeCabCommandHandler.Handle(new RemoveCabCommand(1));
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CustomerRequestRide()
    {
        _customerRideRequestedHandler.Handle(new CustomerRideRequested());
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CustomerCabCall()
    {
        _customerCabRequestedHandler.Handle(
            new CustomerCabRequested("default customer", "1 Fulton Drive", "2 Destination Lane"));
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult SendCabRequest()
    {
        _customerRideRequestedHandler.Handle(new CustomerRideRequested());
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CabNotifiesPickedUp()
    {
        _customerEnroutedHandler.Handle(new CustomerEnrouted());
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CabNotifiesDroppedOff()
    {
        _customerDeliveredHandler.Handle(new CustomerDelivered());
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CustomerCancelledCabRide()
    {
        _customerCancelledCabHandler.Handle(new CustomerCancelledCab());
        
        return RedirectToAction(nameof(Index));
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class FileSettings
{
    public string CustomerFileNameCsv { get; init; } = "customer-file-name.csv";
    public string CabFileNameCsv { get; init; } = "cab-file-name.csv";
}

public class CabDisplayModel
{
    public List<int> DisplayMenu { get; set; }
}