using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddIdentityCore<Auth.ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        //todo set this dynamically
        options.SetIssuer("https://localhost:7032/");
        
        var encryptionKey = builder.Configuration["OpenIddict:EncryptionKey"];
        if (!string.IsNullOrEmpty(encryptionKey))
        {
            options.AddEncryptionKey(new SymmetricSecurityKey(
                Convert.FromBase64String(encryptionKey)));
        }
        
        options.AddAudiences("api");
        options.AddAudiences("next-app");
        

        options.SetClientId("next-app");
        options.SetClientSecret("dev-secret-change");
        
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

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();