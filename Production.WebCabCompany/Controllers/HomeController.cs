using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;
using Production.WebCabCompany.Models;
using Tests.CabDeliveryCompanyKata;

namespace Production.WebCabCompany.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private CabService _cabService;
    private MenuService _menuService;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        var fileHandler = new FileHandler(
            "customer-file-name.csv", 
            "cab-file-name.csv"); // TODO: inject file handler
        var dispatcherCoordinator = new DispatcherCoordinator();
        _cabService = new CabService(dispatcherCoordinator, new CabFileRepository(fileHandler));
        _menuService = new MenuService(dispatcherCoordinator);
    }

    public IActionResult Index()
    {
        var displayMenu = _menuService.DisplayMenu();
        return View(new CabDisplayModel() { DisplayMenu = displayMenu });
    }

    public IActionResult AddCabDriver()
    {
        _cabService.AddCab(new Cab("default", 20, 23.23, 32.32));
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult RemoveCabDriver()
    {
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CustomerRequestRide()
    {
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CustomerCabCall()
    {
        _cabService.CustomerCabCall("default customer", "1 Fulton Drive", "2 Destination Lane");
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult SendCabRequest()
    {
        _cabService.SendCabRequest();
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CabNotifiesPickedUp()
    {
        _cabService.PickupCustomer();
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CabNotifiesDroppedOff()
    {
        _cabService.DropOffCustomer();
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CustomerCancelledCabRide()
    {
        _cabService.CancelPickup();
        
        return RedirectToAction(nameof(Index));
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class CabDisplayModel
{
    public List<int> DisplayMenu { get; set; }
}