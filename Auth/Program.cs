using Auth;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default") ??
                       throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
        options.UseOpenIddict();
    }
);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});

builder.Services.AddOpenIddict()
    .AddCore(options => { options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>(); })
    .AddServer(options =>
    {
        options.SetIssuer(new Uri("https://localhost:7032/"));
        
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
    });

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

if (app.Environment.IsDevelopment())
{
    await OpenIddictSeeder.SeedAsync(app.Services);
}

app.Run();