using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TomatoPizza;
using TomatoPizza.Core.Interfaces;
using TomatoPizza.Core.Services;
using TomatoPizza.Data.Identity;
using TomatoPizza.Data.Repos;
using TomatoPizza.Extensions;
using TomatoPizza.Security;

var builder = WebApplication.CreateBuilder(args);



//Connect to Azure Key Vault (Secrets loaded before anything else)

// We read the Key Vault URL from appsettings.json and store it in a variable.
// Since we're using Managed Identity, only the URL is needed here.
// The rest (like authentication) is handled in Azure automatically.
// NOTE: This only works after the Web API is deployed to Azure.
var keyVaultURL = builder.Configuration["KeyVault:KeyVaultURL"];
var credential = new DefaultAzureCredential();
var secretClient = new SecretClient(new Uri(keyVaultURL!), credential);

// This adds secrets from Azure Key Vault into the IConfiguration system
builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());


//Add configuration for controllers and Swagger (recommended)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();


//JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddAuthorization();

//Use Azure or Local SQL depending on your connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add Identity (User + Roles)
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Dependency Injection for Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Dish feature
// Set up the service for Dependency Injection (DI) for dishes
builder.Services.AddScoped<IDishService, DishService>();
// Register the repository for dish data access
builder.Services.AddScoped<IDishRepo, DishRepo>();

// Ingredient feature
// Set up the service for Dependency Injection (DI) for ingredients
builder.Services.AddScoped<IIngredientService, IngredientService>();
// Register the repository for ingredient data access
builder.Services.AddScoped<IIngredientRepo, IngredientRepo>();

// Order feature
// Set up the service for Dependency Injection (DI) for orders
builder.Services.AddScoped<IOrderService, OrderService>();
// Register the repository for order data access
builder.Services.AddScoped<IOrderRepo, OrderRepo>();

// Enable Application Insights
builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();

//Ensure database is created and seeded with roles and admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    await RoleSeeder.SeedRoles(roleManager);
    await AdminSeeder.SeedAdmin(userManager);
}

//Enable Swagger in Development
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TomatoPizza API V1");
    c.RoutePrefix = string.Empty; // Makes Swagger load at root
});

//Enable Auth Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
