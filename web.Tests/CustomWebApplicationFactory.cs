using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using web.Models.Data;
using web.Seed;

namespace web.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _appConnection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();

                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    // JWT
                    ["Jwt:Issuer"] = "test-issuer",
                    ["Jwt:Audience"] = "test-audience",
                    ["Jwt:Key"] = "THIS_IS_A_TEST_KEY_CHANGE_ME_32CHARS_MINIMUM",
                    ["Jwt:TokenMinutes"] = "60",

                    // Seed users
                    ["Seed:AdminEmail"] = "admin@local.test",
                    ["Seed:AdminPassword"] = "Admin123!",
                    ["Seed:WorkerEmail"] = "worker@local.test",
                    ["Seed:WorkerPassword"] = "Worker123!",
                    ["Seed:UserEmail"] = "user@local.test",
                    ["Seed:UserPassword"] = "User123!",
                    ["Seed:AdminOwnerEmail"] = "adminOwner@local.test",
                    ["Seed:AdminOwnerPassword"] = "adminkoBau123"
                });
            });

            builder.ConfigureServices(services =>
            {
                // Remove production DB
                services.RemoveAll(typeof(DbContextOptions<ApplicationDBContext>));
                services.RemoveAll(typeof(ApplicationDBContext));

                // SQLite in-memory
                _appConnection = new SqliteConnection("DataSource=:memory:");
                _appConnection.Open();

                services.AddDbContext<ApplicationDBContext>(options =>
                    options.UseSqlite(_appConnection));

                // FORCE JwtBearer to use TEST KEY
                services.PostConfigure<JwtBearerOptions>(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = "test-issuer",

                            ValidateAudience = true,
                            ValidAudience = "test-audience",

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    "THIS_IS_A_TEST_KEY_CHANGE_ME_32CHARS_MINIMUM"
                                )
                            ),

                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var db = services.GetRequiredService<ApplicationDBContext>();
            db.Database.EnsureCreated();

            // Seed Identity AFTER schema exists
            IdentitySeeder.SeedAsync(services).GetAwaiter().GetResult();

            return host;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _appConnection?.Close();
                _appConnection?.Dispose();
            }
        }
    }
}
