using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using web.Data;
using web.Models.Data;
using web.Seed;

namespace web.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // ✅ TWO separate in-memory databases (one per DbContext)
        private SqliteConnection? _appConnection;
        private SqliteConnection? _identityConnection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var testConfig = new Dictionary<string, string?>
                {
                    // JWT settings used by token creation + token validation
                    ["Jwt:Issuer"] = "test-issuer",
                    ["Jwt:Audience"] = "test-audience",
                    ["Jwt:Key"] = "THIS_IS_A_TEST_KEY_CHANGE_ME_32CHARS_MINIMUM",

                    // Seed users
                    ["Seed:AdminEmail"] = "admin@local.test",
                    ["Seed:AdminPassword"] = "Admin123!",
                    ["Seed:WorkerEmail"] = "worker@local.test",
                    ["Seed:WorkerPassword"] = "Worker123!",
                    ["Seed:UserEmail"] = "user@local.test",
                    ["Seed:UserPassword"] = "User123!"
                };

                configBuilder.AddInMemoryCollection(testConfig);
            });

            builder.ConfigureServices(services =>
            {
                // Remove production DbContext registrations (Postgres)
                services.RemoveAll(typeof(DbContextOptions<ApplicationDBContext>));
                services.RemoveAll(typeof(ApplicationDBContext));

                services.RemoveAll(typeof(DbContextOptions<AppIdentityDbContext>));
                services.RemoveAll(typeof(AppIdentityDbContext));

                // ✅ Create TWO independent SQLite in-memory DBs
                _appConnection = new SqliteConnection("DataSource=:memory:");
                _appConnection.Open();

                _identityConnection = new SqliteConnection("DataSource=:memory:");
                _identityConnection.Open();

                // App DB (Stocks/Comments)
                services.AddDbContext<ApplicationDBContext>(options =>
                    options.UseSqlite(_appConnection));

                // Identity DB (AspNetUsers/AspNetRoles/etc)
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlite(_identityConnection));
            });
        }

        // DI is ready -> now we can create schema + seed safely
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            // Create schema for BOTH databases (now it works because DBs are separate)
            var appDb = services.GetRequiredService<ApplicationDBContext>();
            appDb.Database.EnsureCreated();

            var identityDb = services.GetRequiredService<AppIdentityDbContext>();
            identityDb.Database.EnsureCreated();

            // Seed identity AFTER schema exists
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

                _identityConnection?.Close();
                _identityConnection?.Dispose();
            }
        }
    }
}
