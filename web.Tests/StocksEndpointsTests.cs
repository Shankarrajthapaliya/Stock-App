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
            LastDiv = 1.23m,
            MarketCap = 1000000,
            Purchase = 10.5m
        };

        
        var response = await _client.PostAsJsonAsync("/api/stock", dto);

        
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


        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

      
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

       
        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginPayload);
        Assert.False(string.IsNullOrWhiteSpace(loginPayload!.Token));

    
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginPayload.Token);

    
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
