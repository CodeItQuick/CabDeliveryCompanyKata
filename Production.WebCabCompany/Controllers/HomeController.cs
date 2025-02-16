using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Application.Menu;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.Menu;
using Production.WebCabCompany.Models;

namespace Production.WebCabCompany.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAddCommandHandler _addCabCommandHandler;
    private readonly IRemoveCabCommandHandler _removeCabCommandHandler;
    private readonly IApplicationHandler _applicationHandler;
    private readonly ICustomerPickedUpHandler _customerPickedUpHandler;
    private readonly ICustomerRideRequestedHandler _customerRideRequestedHandler;
    private readonly IMenuRequestedHandler _menuRequestedHandler;

    public HomeController(ILogger<HomeController> logger,
        IAddCommandHandler addCabCommandHandler,
        IRemoveCabCommandHandler removeCabCommandHandler,
        IApplicationHandler applicationHandler,
        ICustomerPickedUpHandler customerPickedUpHandler,
        ICustomerRideRequestedHandler customerRideRequestedHandler,
        IMenuRequestedHandler menuRequestedHandler)
    {
        _logger = logger;
        _addCabCommandHandler = addCabCommandHandler;
        _removeCabCommandHandler = removeCabCommandHandler;
        _applicationHandler = applicationHandler;
        _customerPickedUpHandler = customerPickedUpHandler;
        _customerRideRequestedHandler = customerRideRequestedHandler;
        _menuRequestedHandler = menuRequestedHandler;
    }

    // TODO: Not tested
    public IActionResult Index()
    {
        var displayMenu = _menuRequestedHandler.Handle(new MenuRequested(1));
        return View(new CabDisplayModel() { DisplayMenu = displayMenu.MenuOptions });
    }

    public IActionResult AddCabDriver()
    {
        _applicationHandler.Handle(new AddCabCommand("default", 23.23, 32.32));
        
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
        _applicationHandler.Handle(
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
        _customerPickedUpHandler.Handle(new CustomerPickedUp());
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CabNotifiesDroppedOff()
    {
        _applicationHandler.Handle(new CustomerDelivered());
        
        return RedirectToAction(nameof(Index));
    }
    public IActionResult CustomerCancelledCabRide()
    {
        _applicationHandler.Handle(new CustomerCancelledCab());
        
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