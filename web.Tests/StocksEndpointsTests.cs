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
        // CreateClient() gives us an HttpClient that calls the in-memory API server.
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_CreateStock_WithoutToken_Returns401()
    {
        // Arrange: build a valid DTO payload
        var dto = new CreateStockDTO
        {
            CompanyName = "Test Company",
            Symbol = "TST",
            Industry = "Testing",
            LastDiv = 1.23m,
            MarketCap = 1000000,
            Purchase = 10.5m
        };

        // Act: call a protected endpoint WITHOUT Authorization header
        var response = await _client.PostAsJsonAsync("/api/stock", dto);

        // Assert: middleware blocks us before controller runs
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_CreateStock_WithValidToken_ReturnsSuccess()
    {
       
        var loginDto = new LogInDTO
        {
            Username = "user@local.test",
            Password = "User123!"
        };

        // This assumes your login endpoint is POST /api/auth/login
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // If login fails, this test should fail loudly.
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        // Example expected login response:
        // { "token": "eyJhbGciOi..." }
        // Adjust property names if your API returns different shape.
        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginPayload);
        Assert.False(string.IsNullOrWhiteSpace(loginPayload!.Token));

        // Attach JWT as Bearer token:
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginPayload.Token);

        // -----------------------------------------------------------
        // STEP 2: Call the protected stock creation endpoint
        // -----------------------------------------------------------
        var dto = new CreateStockDTO
        {
            CompanyName = "Auth Company",
            Symbol = "AUTH",
            Industry = "Testing",
            LastDiv = 0.5m,
            MarketCap = 555,
            Purchase = 12.99m
        };

        var response = await _client.PostAsJsonAsync("/api/stock", dto);

        // -----------------------------------------------------------
        // STEP 3: Assert
        // -----------------------------------------------------------

        // Depending on your controller implementation:
        // - Some return 201 Created
        // - Some return 200 OK
        //
        // Pick the one your API currently does.
        Assert.True(
            response.StatusCode == HttpStatusCode.Created ||
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 200 or 201 but got {(int)response.StatusCode}"
        );
    }

    /// <summary>
    /// Helper type for parsing login response JSON.
    /// If your API returns different fields, change this class accordingly.
    /// </summary>
    private sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
    }
