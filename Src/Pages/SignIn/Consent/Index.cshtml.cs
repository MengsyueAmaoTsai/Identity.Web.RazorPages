using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignIn.Consent;

public sealed class SignInConsentViewModel() :
    IdentityViewModel
{
    [BindProperty]
    public required string Description { get; init; }

    [BindProperty]
    public required bool RememberMyDecision { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            return ErrorPage();
        }

        return Page();
    }
}