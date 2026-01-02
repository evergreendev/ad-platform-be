using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Microsoft.AspNetCore.DataProtection;
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

builder.Services.AddDataProtection()
    .SetApplicationName("Ad-platform")
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "..", "shared-keys")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<Auth.ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender<Auth.ApplicationUser>, Auth.Services.EmailSender>();

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        var issuer = builder.Configuration["OpenIddict:Issuer"];

        if (!string.IsNullOrEmpty(issuer))
        {
            options.SetIssuer(issuer);
        }
        
        var encryptionKey = builder.Configuration["OpenIddict:EncryptionKey"];
        if (!string.IsNullOrEmpty(encryptionKey))
        {
            options.AddEncryptionKey(new SymmetricSecurityKey(
                Convert.FromBase64String(encryptionKey)));
        }
        
        options.AddAudiences("api");
        options.AddAudiences("next-app");
        

        options.SetClientId("next-app");
        
        var clientSecret = builder.Configuration["OpenIddict:ClientSecret"];

        if (!string.IsNullOrEmpty(clientSecret))
        {
            options.SetClientSecret(clientSecret);
        }
        
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