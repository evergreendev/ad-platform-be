using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ad_platform.Data;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default") ??
                       throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    }
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    { 
        //todo set this dynamically
        options.SetIssuer("https://localhost:7032/");
        
        options.AddAudiences("api");

        options.SetClientId("next-app");
        options.SetClientSecret("dev-secret-change");

        options.UseIntrospection();
        options.UseSystemNetHttp();
        
        options.UseAspNetCore();
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Example protected endpoint
app.MapGet("/whoami", (System.Security.Claims.ClaimsPrincipal user) =>
    {
        return user.Claims.Select(c => new { c.Type, c.Value });
    })
    .RequireAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();