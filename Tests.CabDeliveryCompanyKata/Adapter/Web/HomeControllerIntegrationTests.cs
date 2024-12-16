using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Application.Menu;
using Production.WebCabCompany.Controllers;

namespace Tests.CabDeliveryCompanyKata.Adapter.Web;

public class HomeControllerIntegrationTests
{
    private CabContext _cabContext;
    private FleetRepository _fleetRepository;
    private AddCabCommandHandler _addCabCommandHandler;
    private HomeController _homeController;

    public HomeControllerIntegrationTests()
    {
        var cabContextOptions = new DbContextOptionsBuilder<CabContext>()
            .UseSqlite($"Data Source={Guid.NewGuid()}");

        _cabContext = new CabContext(cabContextOptions.Options);
        _cabContext.Database.Migrate();
        _fleetRepository = new FleetRepository(_cabContext);
        _addCabCommandHandler = new AddCabCommandHandler(_fleetRepository);
        var fileSettings = new FileSettings()
        {
            CabFileNameCsv = $"{Guid.NewGuid().ToString()}.csv",
            CustomerFileNameCsv = $"{Guid.NewGuid().ToString()}.csv"
        };
        IOptions<FileSettings> options = Options.Create(fileSettings);
        _homeController = new HomeController(
            new NullLogger<HomeController>(),
            _addCabCommandHandler,
            new RemoveCabCommandHandler(new FleetRepository(_cabContext)),
            new ApplicationHandler(
                new CustomerCabRequestedHandler(new CustomerListRepository(_cabContext)),
                new CustomerCancelledCabHandler(new CustomerListRepository(_cabContext)),
                new CustomerPickedUpHandler(new CustomerListRepository(_cabContext)),
                new CustomerDeliveredHandler(new CustomerListRepository(_cabContext))
            ),
            new CustomerPickedUpHandler(new CustomerListRepository(_cabContext)),
            new CustomerRideRequestedHandler(new CustomerListRepository(_cabContext)),
            new MenuRequestedHandler(new MenuRepository(_cabContext)));
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