using System.Net;
using System.Net.Http.Json;
using CookbookWebApi.Models;
using Xunit;

namespace CookbookTests.Controllers;

public class AuthController_DebugTests 
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthController_DebugTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturnToken_WhenRequestIsValid()
    {
        var request = new RegisterRequest
        {
            Email = "test@test.com",
            FullName = "Test User",
            Password = "Qwerty123!@#",
            ConfirmPassword = "Qwerty123!@#"
        };

        var response = await _client.PostAsJsonAsync(
            "/cookbook/Auth/Register", request);

        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var registerRequest = new RegisterRequest
        {
            Email = "login@test.com",
            FullName = "Login User",
            Password = "Qwerty123!@#",
            ConfirmPassword = "Qwerty123!@#"
        };

        await _client.PostAsJsonAsync(
            "/cookbook/Auth/Register", registerRequest);

        var loginRequest = new LoginRequest
        {
            Email = registerRequest.Email,
            Password = registerRequest.Password
        };

        var response = await _client.PostAsJsonAsync(
            "/cookbook/Auth/Login", loginRequest);

        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Profile_ShouldReturnCurrentUser_WhenTokenIsValid()
    {
        var registerRequest = new RegisterRequest
        {
            Email = "profile@test.com",
            FullName = "Profile User",
            Password = "Qwerty123!@#",
            ConfirmPassword = "Qwerty123!@#"
        };

        var registerResponse = await _client.PostAsJsonAsync(
            "/cookbook/Auth/Register", registerRequest);

        var auth = await registerResponse.Content
            .ReadFromJsonAsync<AuthResponse>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", auth!.Token);

        var response = await _client.GetAsync(
            "/cookbook/Auth/Profile");

        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnUser_WhenUserExists()
    {
        var registerRequest = new RegisterRequest
        {
            Email = "get@test.com",
            FullName = "Get User",
            Password = "Qwerty123!@#",
            ConfirmPassword = "Qwerty123!@#"
        };

        var registerResponse = await _client.PostAsJsonAsync(
            "/cookbook/Auth/Register", registerRequest);

        var auth = await registerResponse.Content
            .ReadFromJsonAsync<AuthResponse>();

        var response = await _client.GetAsync(
            $"/cookbook/Auth/Get?userId={auth!.UserId}");

        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
