using System.Collections.Immutable;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Auth.Controllers;

public class AuthorizationController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
    : Controller
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
                      ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (User.Identity?.IsAuthenticated != true)
        {
            var returnUrl = Request.PathBase + Request.Path + Request.QueryString;

            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                });
        }
        
        // Resolve the current user.
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            // If the cookie is stale or user was deleted, force re-login.
            await signInManager.SignOutAsync();

            var returnUrl = Request.PathBase + Request.Path + Request.QueryString;

            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                });
        }
        
        var principal = await signInManager.CreateUserPrincipalAsync(user);
        
        principal.SetClaim(Claims.Subject, await userManager.GetUserIdAsync(user));
        
        var identifier = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);
        
        var scopes = request.GetScopes();
        
        identity.SetScopes(scopes);
        identity.SetResources("api");

        if (scopes.Contains(Scopes.Roles))
        {
                    var roles = await userManager.GetRolesAsync(user);
                    
                    identity.SetClaims(Claims.Role, roles.ToImmutableArray());
        }
        

        
        // Import a few select claims from the identity stored in the local cookie.
        identity.AddClaim(new Claim(Claims.Subject, identifier));
        identity.AddClaim(new Claim(Claims.Name, identifier).SetDestinations(Destinations.AccessToken));
        identity.AddClaim(new Claim(Claims.PreferredUsername, identifier).SetDestinations(Destinations.AccessToken));
        
        identity.SetClaim(Claims.Name, user.UserName);
        identity.SetClaim(Claims.Email, user.Email);
        
        // Apply requested scopes (you can also restrict these if you want).
        identity.SetScopes(request.GetScopes());
        identity.SetResources("api");
        
        // Decide which claims go into which tokens.
        foreach (var claim in identity.Claims)
        {
            claim.SetDestinations(GetDestinations(claim));
        }

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
    
    // Token endpoint: exchanges auth code for tokens, and refreshes tokens.
    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
                      ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // These are the grant types you'll see with code flow + refresh tokens.
        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
        {
            return BadRequest(new { error = "unsupported_grant_type" });
        }

        // Authenticate the token request with OpenIddict to retrieve the principal
        // stored in the authorization code / refresh token.
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (!result.Succeeded || result.Principal is null)
        {
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var principal = result.Principal;
        
        // Re-apply destinations (safe; avoids missing claim destinations issues).
        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetDestinations(claim));
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
    
    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        return claim.Type switch
        {
            Claims.Name or Claims.Email or Claims.Role =>
                new[] { Destinations.AccessToken, Destinations.IdentityToken },

            _ => new[] { Destinations.AccessToken }
        };
    }
}