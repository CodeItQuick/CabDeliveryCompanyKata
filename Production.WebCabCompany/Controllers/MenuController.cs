using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.EmmaCabCompany.Application.Menu;
using Production.EmmaCabCompany.Domain.Menu;
using Production.WebCabCompany.Models;

namespace Production.WebCabCompany.Controllers;

[Authorize]
public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly IMenuRequestedHandler _menuRequestedHandler;

    public MenuController(ILogger<MenuController> logger,
        CabContext cabContext)
    {
        _logger = logger;
        _menuRequestedHandler = new MenuRequestedHandler(new MenuRepository(cabContext));
    }

    // TODO: Not tested
    public IActionResult Index()
    {
        var displayMenu = _menuRequestedHandler.Handle(new MenuRequested(1));
        return View(new CabDisplayModel() { DisplayMenu = displayMenu.MenuOptions });
    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


public class CabDisplayModel
{
    public List<string> DisplayMenu { get; set; }
}