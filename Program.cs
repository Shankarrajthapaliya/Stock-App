using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Interface;
using web.Models.Data;
using web.Repo;
using web.Services;
using Microsoft.AspNetCore.Identity;
using web.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using web.Seed;
using System.Security.Claims;




var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<SeedOptions>(builder.Configuration.GetSection("Seed"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add JWT Bearer support to Swagger UI
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddDbContext<ApplicationDBContext> (options => 
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]!;
var issuer = jwtSection["Issuer"]!;
var audience = jwtSection["Audience"]!;

builder.Services.AddAuthentication(options =>
{
    // This tells ASP.NET Core "use JWT bearer as default auth"
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // These settings define how the middleware validates incoming JWTs
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,

        ValidateAudience = true,
        ValidAudience = audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),

       
        ValidateLifetime = true,

    
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanDeleteStock", policy =>
        policy.RequireClaim("permission", "delete:stock"));
});

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


builder.Services.AddScoped<IStockRepo,StockRepository>();

builder.Services.AddScoped<ICommentRepo, CommentRepository>();
builder.Services.AddScoped<IPortfolioRepo,PortfolioRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

//builder.Services.AddControllers();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    if (!app.Environment.IsEnvironment("Testing"))
    {
        await IdentitySeeder.SeedAsync(services);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication(); 
app.UseAuthorization();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}
app.MapControllers();
app.Run();

public partial class Program(); 









