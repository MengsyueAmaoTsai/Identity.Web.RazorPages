using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

public sealed class ChallengeViewModel(
    ILogger<ChallengeViewModel> _logger,
    IIdentityServerInteractionService _interactionService) :
    PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; set; } = "~/";

    [BindProperty(SupportsGet = true)]
    public required string AuthenticationProvider { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            ReturnUrl = "~/";
        }

        if (!Url.IsLocalUrl(ReturnUrl) && !_interactionService.IsValidReturnUrl(ReturnUrl))
        {
            throw new Exception("Invalid return URL");
        }

        var redirectUri = Url.Page("./callback");
        _logger.LogInformation("Redirect Uri: {RedirectUri}", redirectUri);

        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUri,
            Items =
            {
                { "returnUrl", ReturnUrl },
                { "scheme", AuthenticationProvider },
            },
        };

        return Challenge(properties, AuthenticationProvider);
    }
}