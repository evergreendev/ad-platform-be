using Auth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default") ??
                       throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
        options.UseOpenIddict();
    }
);

builder.Services.AddDataProtection()
    .SetApplicationName("Ad-platform")
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "..", "shared-keys")));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});

builder.Services.AddOpenIddict()
    .AddCore(options => { options.UseEntityFrameworkCore().UseDbContext<AuthDbContext>(); })
    .AddServer(options =>
    {
        options.SetIssuer(new Uri(builder.Configuration["OpenIddict:Issuer"]!));
        
        options.SetAuthorizationEndpointUris("/connect/authorize")
            .SetTokenEndpointUris("/connect/token")
            .SetIntrospectionEndpointUris("/connect/introspect");

        options.AllowAuthorizationCodeFlow();

        options.RequireProofKeyForCodeExchange();

        options.AllowRefreshTokenFlow();
                // Register the encryption credentials. This sample uses a symmetric
        // encryption key that is shared between the server and the API project.
        var encryptionKey = builder.Configuration["OpenIddict:EncryptionKey"];
        if (!string.IsNullOrEmpty(encryptionKey))
        {
            options.AddEncryptionKey(new SymmetricSecurityKey(
                Convert.FromBase64String(encryptionKey)));
        }
        
        if (builder.Environment.IsDevelopment())
        {
            options.AddDevelopmentSigningCertificate();
            options.AddDevelopmentEncryptionCertificate(); // Usually paired together
        }

        options.RegisterScopes(
            OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Roles,
            "api"
        );

        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseAspNetCore();
        options.UseLocalServer();
        options.AddAudiences("api");
        options.AddAudiences("next-app");
    });

builder.Services.AddTransient<IEmailSender<ApplicationUser>, Auth.Services.EmailSender>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    // Optional but often needed behind proxies:
    // options.KnownNetworks.Clear();
    // options.KnownProxies.Clear();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

var clientSecret = builder.Configuration["OpenIddict:ClientSecret"];

if (app.Environment.IsDevelopment() && !string.IsNullOrEmpty(clientSecret))
{
    await OpenIddictSeeder.SeedAsync(app.Services, clientSecret);
}

app.Run();