using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
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
            _cookieContainerHandler
            // new RedirectHandler()
        );
    }

    [Fact]
    public void CanAddCabDriverToFleet()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);
        NavigateSecurelyTo($"Manage/Index", applicationToken);
        
        var response = NavigateSecurelyTo($"Home/AddCabDriver", applicationToken);
        
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }
    [Fact]
    public void CanRequestCustomerRide()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);
        NavigateSecurelyTo($"Manage/Index", applicationToken);
        NavigateSecurelyTo($"Home/AddCabDriver", applicationToken);
        NavigateSecurelyTo($"Home/CustomerCabCall", applicationToken);
        var response = NavigateSecurelyTo($"Home/SendCabRequest", applicationToken);
        
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }
    [Fact]
    public void CanRequestCustomerRideAndSendCabRequest()
    {
        
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);
        NavigateSecurelyTo($"Manage/Index", applicationToken);
        NavigateSecurelyTo($"Home/AddCabDriver", applicationToken);
        NavigateSecurelyTo($"Home/CustomerCabCall", applicationToken)
            ;
        var response = NavigateSecurelyTo($"Home/SendCabRequest", applicationToken);
        
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }
    [Fact]
    public void CanPickupFare()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);
        NavigateSecurelyTo($"Manage/Index", applicationToken);
        NavigateSecurelyTo($"Home/AddCabDriver", applicationToken);
        NavigateSecurelyTo($"Home/CustomerCabCall", applicationToken);
        NavigateSecurelyTo($"Home/SendCabRequest", applicationToken);
        var response = NavigateSecurelyTo($"Home/CabNotifiesPickedUp", applicationToken);
        
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }

    [Fact]
    public void CanDropOffFare()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);
        NavigateSecurelyTo($"Manage/Index", applicationToken);
        NavigateSecurelyTo($"Home/AddCabDriver", applicationToken);
        NavigateSecurelyTo($"Home/CustomerCabCall", applicationToken);
        NavigateSecurelyTo($"Home/SendCabRequest", applicationToken);
        NavigateSecurelyTo($"Home/CabNotifiesPickedUp", applicationToken);

        var response = NavigateSecurelyTo($"Home/CabNotifiesDroppedOff", applicationToken);
        
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }

    [Fact]
    public void CanCancelCabFare()
    {
        
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);
        NavigateSecurelyTo($"Manage/Index", applicationToken);
        NavigateSecurelyTo($"Home/AddCabDriver", applicationToken);
        NavigateSecurelyTo($"Home/CustomerCabCall", applicationToken);
        
        var response = NavigateSecurelyTo($"Home/CustomerCancelledCabRide", applicationToken);
        
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }
    
    private HttpResponseMessage NavigateSecurelyTo(string webpage, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(IdentityConstants.ApplicationScheme, token);
        var httpResponseMessage = _httpClient.GetAsync(webpage).Result;
        return httpResponseMessage;
    }

    private HttpResponseMessage ChangePassword(string password, string antiForgeryToken)
    {
        var loginAttempt = _httpClient.PostAsync("/Account/Login",
                new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "OldPassword", "Password.1" },
                    { "NewPassword", password },
                    { "ConfirmPassword", password },
                    { "__RequestVerificationToken", antiForgeryToken },
                }))
            .Result;
        return loginAttempt;
    }
    
    private string NavigateToPage(string pageAddress, string? applicationToken)
    {
        if (applicationToken != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(IdentityConstants.ApplicationScheme, applicationToken);
        }
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
    private string DefaultRegister(string antiForgeryToken, string username)
    {
        var loginAttempt = _httpClient.PostAsync("/Account/Register",
                new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "Email", username },
                    { "Password", "Password.1" },
                    { "ConfirmPassword", "Password.1" },
                    { "RememberMe", "false" },
                    { "__RequestVerificationToken", antiForgeryToken },
                }))
            .Result;

        var content = loginAttempt.Headers.ToString();
        var identityApplicationToken = content.Split("=")[1];
        var identityToken = identityApplicationToken.Split(";")[0];
        return identityToken;
    }
}