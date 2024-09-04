using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Production.WebCabCompany.Controllers;

namespace Tests.CabDeliveryCompanyKata.Adapter.Web;

public class HomeControllerIntegrationTests
{
    private HomeController _homeController;

    public HomeControllerIntegrationTests()
    {
        var fileSettings = new FileSettings()
        {
            CabFileNameCsv = $"{Guid.NewGuid().ToString()}.csv",
            CustomerFileNameCsv = $"{Guid.NewGuid().ToString()}.csv"
        };
        IOptions<FileSettings> options = Options.Create(fileSettings);
        _homeController = new HomeController(new NullLogger<HomeController>(), options);
        
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

        var response = _homeController.Index() as ViewResult;

        Assert.Equal(4, (response!.Model as CabDisplayModel)!.DisplayMenu.Count);
        Assert.Equivalent(
            (response!.Model as CabDisplayModel)!.DisplayMenu.ToArray(), 
            (int[]) [0, 1, 2, 7]);
    }
}