using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.DTO;
using Microsoft.AspNetCore.Mvc.Testing;

namespace web.Tests
{
    public class StocksEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        

    private readonly HttpClient _client;

    public StocksEndpointsTests(CustomWebApplicationFactory factory)
    {
      
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_CreateStock_WithoutToken_Returns401()
    {
   
        var dto = new CreateStockDTO
        {
            CompanyName = "Test Company",
            Symbol = "TST",
            Industry = "Testing",
            MarketCap = 1000000,
        };

        
        var response = await _client.PostAsJsonAsync("/api/stock", dto);

        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
public async Task Post_CreateStock_WithValidToken_ReturnsSuccess()
{
    // --- LOGIN ---
    var loginDto = new LogInDTO
    {
        Username = "admin@local.test",
        Password = "Admin123!"
    };

    var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
    Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

    var loginPayload = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
    Assert.NotNull(loginPayload);
    Assert.False(string.IsNullOrWhiteSpace(loginPayload!.Token));

    // --- DEBUG JWT ---
    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(loginPayload.Token);

    Console.WriteLine("===== JWT DEBUG =====");
    Console.WriteLine($"iss: {jwt.Issuer}");
    Console.WriteLine($"aud: {string.Join(",", jwt.Audiences)}");
    Console.WriteLine($"validFrom: {jwt.ValidFrom:o}");
    Console.WriteLine($"validTo:   {jwt.ValidTo:o}");
    Console.WriteLine("=====================");

    // --- ATTACH TOKEN ---
    _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", loginPayload.Token);

    // --- CALL PROTECTED ENDPOINT ---
    var dto = new CreateStockDTO
    {
        CompanyName = "Auth Company",
        Symbol = "AUTH",
        Industry = "Testing",
        MarketCap = 555,
        
    };

    var response = await _client.PostAsJsonAsync("/api/stock", dto);

    // --- DEBUG AUTH FAILURE ---
    Console.WriteLine("===== API RESPONSE =====");
    Console.WriteLine($"StatusCode: {(int)response.StatusCode}");
    Console.WriteLine($"WWW-Authenticate: {response.Headers.WwwAuthenticate}");
    Console.WriteLine(await response.Content.ReadAsStringAsync());
    Console.WriteLine("========================");

    Assert.True(
        response.StatusCode == HttpStatusCode.Created ||
        response.StatusCode == HttpStatusCode.OK,
        $"Expected 200 or 201 but got {(int)response.StatusCode}"
    );
}
    private sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
    }
