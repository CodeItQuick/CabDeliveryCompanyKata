using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.Menu;
using Production.WebCabCompany.Models;

namespace Production.WebCabCompany.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IApplicationHandler _applicationHandler;

    public HomeController(ILogger<HomeController> logger,
        CabContext cabContext)
    {
        _logger = logger;
        _applicationHandler = new ApplicationHandler(cabContext);
    }

    public IActionResult AddCabDriver()
    {
        var addCabCommand = new AddCabCommand("default", 23.23, 32.32);
        _applicationHandler.Handle(addCabCommand);
        
        return Redirect($"/Menu/Index");
    }
    public IActionResult RemoveCabDriver()
    {
        _applicationHandler.Handle(new RemoveCabCommand(1));
        return Redirect($"/Menu/Index");
    }
    public IActionResult CustomerRequestRide()
    {
        _applicationHandler.Handle(new CustomerRideRequested());
        return Redirect($"/Menu/Index");
    }
    public IActionResult CustomerCabCall()
    {
        _applicationHandler.Handle(
            new CustomerCabRequested("default customer", "1 Fulton Drive", "2 Destination Lane"));
        
        return Redirect($"/Menu/Index");
    }
    public IActionResult SendCabRequest()
    {
        _applicationHandler.Handle(new CustomerRideRequested());
        
        return Redirect($"/Menu/Index");
    }
    public IActionResult CabNotifiesPickedUp()
    {
        _applicationHandler.Handle(new CustomerPickedUp());
        
        return Redirect($"/Menu/Index");
    }
    public IActionResult CabNotifiesDroppedOff()
    {
        _applicationHandler.Handle(new CustomerDelivered());
        
        return Redirect($"/Menu/Index");
    }
    public IActionResult CustomerCancelledCabRide()
    {
        _applicationHandler.Handle(new CustomerCancelledCab());
        
        return Redirect($"/Menu/Index");
    }
    
    //
    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}