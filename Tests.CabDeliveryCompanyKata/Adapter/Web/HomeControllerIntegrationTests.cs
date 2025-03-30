using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.WebCabCompany.Controllers;

namespace Tests.CabDeliveryCompanyKata.Adapter.Web;

public class HomeControllerIntegrationTests
{
    private CabContext _cabContext;
    private HomeController _homeController;
    private MenuController _menuController;

    public HomeControllerIntegrationTests()
    {
        var cabContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");

        _cabContext = new CabContext(cabContextOptions.Options);
        _cabContext.Database.Migrate();
        EnsureFleetExistsForSingleUser();
        EnsureMenuExistsForSingleUser();
        _homeController = new HomeController(
            new NullLogger<HomeController>(),
            _cabContext);
        _menuController = new MenuController(new NullLogger<MenuController>(), _cabContext);
        var claimsIdentity = new ClaimsIdentity(
            new List<Claim>()
            {
                new(ClaimTypes.Name, "test_username"),
                new(ClaimTypes.NameIdentifier, "abcd-1234"),
                new(ClaimTypes.Role, "Admin"),
            },
            "TestAuthType");
        _homeController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(claimsIdentity)
            }
        };
    }

    [Fact]
    public void CanAddCabDriverToFleet()
    {
        
        _homeController.AddCabDriver();

        var response = _menuController.Index() as ViewResult;

        Assert.Equal(4, (response!.Model as CabDisplayModel)!.DisplayMenu.Count);
        Assert.Equivalent(
            (response!.Model as CabDisplayModel)!.DisplayMenu.ToArray(), 
            (string[]) ["0", "1", "8", "9"]);
    }
    
    private void EnsureFleetExistsForSingleUser()
    {
        var fleetExists = _cabContext.Fleet.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Fleet.Add(new Fleet() { Id = 1 });
        _cabContext.SaveChanges();
    }

    private void EnsureMenuExistsForSingleUser()
    {
        var fleetExists = _cabContext.Menu.Any(x => x.Id == 1);
        if (fleetExists) return;
        _cabContext.Menu.Add(new Menu() { Id = 1 });
        _cabContext.SaveChanges();
    }

    public void EmptyFleet(int fleetId)
    {
        var fleet = _cabContext.Fleet.Include(x => x.FleetOfCabs)
            .FirstOrDefault(x => x.Id == fleetId);
        _cabContext.CabDrivers.RemoveRange(fleet!.FleetOfCabs.ToList());
        _cabContext.SaveChanges();
    }
}