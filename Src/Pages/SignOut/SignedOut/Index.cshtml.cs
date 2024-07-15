using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignOut.SignedOut;

[AllowAnonymous]
public sealed class SignedOutViewModel : ViewModel
{
    [BindProperty(Name = "postLogoutRedirectUri", SupportsGet = true)]
    public required string PostLogoutRedirectUri { get; init; }

    public IActionResult OnGet()
    {
        return Page();
    }
}