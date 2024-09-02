using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Tests.CabDeliveryCompanyKata.Adapter.Web;

public class AccountControllerTests : IClassFixture<TestingWebApiFactory>
{
    private readonly TestingWebApiFactory _factory;
    private readonly CookieContainerHandler _cookieContainerHandler;
    private readonly HttpClient _httpClient;

    public AccountControllerTests(TestingWebApiFactory factory)
    {
        _factory = factory;
        _cookieContainerHandler = new CookieContainerHandler();
        _httpClient = _factory.CreateDefaultClient(
            _cookieContainerHandler
            // new RedirectHandler()
        );
    }

    [Fact]
    public void CanRegisterNewUser()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);

        var applicationToken = DefaultRegister(antiForgeryToken, Guid.NewGuid() + "@test.com");
        Assert.NotEmpty(applicationToken);
    }

    [Fact]
    public void CanLoginExistingUser()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);

        var response = DefaultLogin(loginToken, loginUsername);

        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }

    [Fact]
    public void CanNavigateToManageAfterLogin()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);

        var response = NavigateSecurelyTo($"Manage/Index", applicationToken);
    
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public void CanChangePassword()
    {
        var antiForgeryToken = NavigateToPage("Account/Register", null);
        var loginUsername = Guid.NewGuid() + "@test.com";
        var applicationToken = DefaultRegister(antiForgeryToken, loginUsername);
        var loginToken = NavigateToPage("Account/Login", applicationToken);
        DefaultLogin(loginToken, loginUsername);
        _ = NavigateSecurelyTo($"Manage/Index", applicationToken);
        var antiforgeryTokenTwo = NavigateToPage($"Manage/ChangePassword", applicationToken);
    
        var response = ChangePassword("Password.2", antiforgeryTokenTwo);
    
        Assert.True(response.IsSuccessStatusCode);
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