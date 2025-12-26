using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Auth;

public class OpenIddictSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await scopeManager.FindByNameAsync("api") is null)
        {
            var apiScope = new OpenIddictScopeDescriptor
            {
                Name = "api",
                Resources = { "api" }
            };
            
            await scopeManager.CreateAsync(apiScope);
        }

        var clientId = "next-app";
        var redirectUri = new Uri("http://localhost:3000/api/auth/callback/openiddict");
        var postLogoutRedirectUri = new Uri("https://localhost:3000");

        if (await appManager.FindByClientIdAsync(clientId) is null)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = "dev-secret-change",
                DisplayName = "Next App (Dev)",

                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,

                RedirectUris = { redirectUri },
                PostLogoutRedirectUris = { postLogoutRedirectUri },

                Permissions =
                {
                    // endpoints
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,

                    // grant types
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                    // response types
                    OpenIddictConstants.Permissions.ResponseTypes.Code,

                    // scopes
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                },

                Requirements =
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                }
            };
            
            await appManager.CreateAsync(descriptor);
        }

        var devEmail = "joe@egmrc.com";
        var devPassword = "Pass1234!";
        
        var user = await userManager.FindByEmailAsync(devEmail);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = devEmail,
                Email = devEmail,
                EmailConfirmed = true,
            };
            
            var result = await userManager.CreateAsync(user, devPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException("Failed to create dev user: " + errors);
            }
        }

    }
}