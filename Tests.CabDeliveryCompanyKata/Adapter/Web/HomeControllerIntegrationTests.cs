using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Production.EmmaCabCompany.Adapter.@out.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.WebCabCompany.Controllers;

namespace Tests.CabDeliveryCompanyKata.Adapter.Web;

public class HomeControllerIntegrationTests
{
    private CabContext _cabContext;
    private FleetRepository _fleetRepository;

    public HomeControllerIntegrationTests()
    {
        var cabContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");

        _cabContext = new CabContext(cabContextOptions.Options);
        _cabContext.Database.Migrate();
        _fleetRepository = new FleetRepository(_cabContext);
    }

    [Fact]
    public void CanAddCabDriverToFleet()
    {
        var fileSettings = new FileSettings()
        {
            CabFileNameCsv = $"{Guid.NewGuid().ToString()}.csv",
            CustomerFileNameCsv = $"{Guid.NewGuid().ToString()}.csv"
        };
        IOptions<FileSettings> options = Options.Create(fileSettings);
        var homeController = new HomeController(
            new NullLogger<HomeController>(), 
            options, 
            _fleetRepository);
        
        var claimsIdentity = new ClaimsIdentity(
            new List<Claim>()
            {
                new(ClaimTypes.Name, "test_username"),
                new(ClaimTypes.NameIdentifier, "abcd-1234"),
                new(ClaimTypes.Role, "Admin"),
            },
            "TestAuthType");
        homeController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(claimsIdentity)
            }
        };
        homeController.AddCabDriver();

        var response = homeController.Index() as ViewResult;

        Assert.Equal(4, (response!.Model as CabDisplayModel)!.DisplayMenu.Count);
        Assert.Equivalent(
            (response!.Model as CabDisplayModel)!.DisplayMenu.ToArray(), 
            (int[]) [0, 1, 2, 7]);
    }
}