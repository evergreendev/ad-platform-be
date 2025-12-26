using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Auth.Controllers;

public class AuthorizationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthorizationController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

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

            var loginUrl = Url.Page(
                pageName: "/Account/Login",
                pageHandler: null,
                values: new { area = "Identity", returnUrl },
                protocol: Request.Scheme);
            
            return Redirect(loginUrl!);
        }
        
        // Resolve the current user.
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            // If the cookie is stale or user was deleted, force re-login.
            await _signInManager.SignOutAsync();

            var returnUrl = Request.PathBase + Request.Path + Request.QueryString;
            var loginUrl = Url.Page(
                "/Account/Login",
                pageHandler: null,
                values: new { area = "Identity", returnUrl },
                protocol: Request.Scheme);

            return Redirect(loginUrl!);
        }
        
        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        
        principal.SetClaim(Claims.Subject, await _userManager.GetUserIdAsync(user));
        principal.SetClaim(Claims.Name, user.UserName);
        principal.SetClaim(Claims.Email, user.Email);
        
        // Apply requested scopes (you can also restrict these if you want).
        principal.SetScopes(request.GetScopes());
        principal.SetResources("api");
        
        // Decide which claims go into which tokens.
        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetDestinations(claim));
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
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
            Claims.Name or Claims.Email =>
                new[] { Destinations.AccessToken, Destinations.IdentityToken },

            _ => new[] { Destinations.AccessToken }
        };
    }
}