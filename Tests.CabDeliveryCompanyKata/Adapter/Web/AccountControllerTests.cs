using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;

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
            _cookieContainerHandler,
            new RedirectHandler()
        );
    }

    [Fact]
    public void CanLoginExistingUser()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");

        var response = DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");

        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact]
    public void CanRegisterNewUser()
    {
        var antiForgeryToken = NavigateToPage("Account/Register");

        var response = DefaultLogin(antiForgeryToken, Guid.NewGuid() + "@test.com");

        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact]
    public void CanNavigateToManageAfterLogin()
    {
        var antiForgeryToken = NavigateToPage("Account/Login");
        DefaultLogin(antiForgeryToken, "evanontario009@gmail.com");

        var response = _httpClient.GetAsync($"Manage/Index").Result;

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