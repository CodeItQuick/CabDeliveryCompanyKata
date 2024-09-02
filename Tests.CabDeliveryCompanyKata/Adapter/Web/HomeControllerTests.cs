using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Tests.CabDeliveryCompanyKata.Adapter.Web;

public class HomeControllerTests : IClassFixture<TestingWebApiFactory>
{
    private readonly TestingWebApiFactory _factory;
    private readonly CookieContainerHandler _cookieContainerHandler;
    private readonly HttpClient _httpClient;

    public HomeControllerTests(TestingWebApiFactory factory)
    {
        _factory = factory;
        _cookieContainerHandler = new CookieContainerHandler();
        _httpClient = _factory.CreateDefaultClient(
            _cookieContainerHandler,
            new RedirectHandler()
        );
    }

    [Fact]
    public void CanAddCabDriverToFleet()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");
        DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");

        var response = _httpClient.GetAsync($"Home/AddCabDriver").Result;

        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact]
    public void CanRequestCustomerRide()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");
        DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");
        NavigateToPage($"Home/AddCabDriver");

        var response = _httpClient.GetAsync($"Home/CustomerCabCall").Result;

        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact]
    public void CanRequestCustomerRideAndSendCabRequest()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");
        DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");
        NavigateToPage($"Home/AddCabDriver");
        NavigateToPage($"Home/CustomerCabCall");

        var response = _httpClient.GetAsync($"Home/SendCabRequest").Result;

        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact]
    public void CanPickupFare()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");
        DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");
        NavigateToPage($"Home/AddCabDriver");
        NavigateToPage($"Home/CustomerCabCall");
        NavigateToPage($"Home/SendCabRequest");

        var response = _httpClient.GetAsync($"Home/CabNotifiesPickedUp").Result;

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public void CanDropOffFare()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");
        DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");
        NavigateToPage($"Home/AddCabDriver");
        NavigateToPage($"Home/CustomerCabCall");
        NavigateToPage($"Home/SendCabRequest");
        NavigateToPage($"Home/CabNotifiesPickedUp");

        var response = _httpClient.GetAsync($"Home/CabNotifiesDroppedOff").Result;

        Assert.Contains("Home Page", response.Content.ReadAsStringAsync().Result);
    }

    [Fact]
    public void CanCancelCabFare()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");
        DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");
        NavigateToPage($"Home/AddCabDriver");
        NavigateToPage($"Home/CustomerCabCall");

        var response = _httpClient.GetAsync($"Home/CustomerCancelledCabRide").Result;

        Assert.True(response.IsSuccessStatusCode);
    }
    
    private string NavigateToPage(string? pageAddress)
    {
        var content = _httpClient.GetAsync(pageAddress)
            .Result
            .Content
            .ReadAsStringAsync()
            .Result;
        var forgeryToken = Regex
            .Match(content, "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(.*?)\"")
            .Groups[1].Value;
        return forgeryToken;
    }

    private HttpResponseMessage DefaultLogin(string antiForgeryToken, string username)
    {
        var loginAttempt = _httpClient.PostAsync("/Account/Login",
                new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "Email", username },
                    { "Password", "Password.1" },
                    { "RememberMe", "false" },
                    { "__RequestVerificationToken", antiForgeryToken },
                }))
            .Result;
        return loginAttempt;
    }
}