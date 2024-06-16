using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity;

[Authorize]
public sealed class SignOutViewModel : PageModel
{
    public async Task<IActionResult> OnPostAsync(string returnUrl, CancellationToken cancellationToken = default)
    {
        var result = await SignOutAsync(cancellationToken);

        if (result.IsFailure)
        {
            return Page();
        }

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            // This needs to be a redirect so that the browser performs a new
            // request and the identity for the user gets updated.
            return RedirectToPage();
        }
    }

    private async Task<Result> SignOutAsync(
        CancellationToken cancellationToken = default)
    {
        await HttpContext.SignOutAsync(RichillCapitalAuthenticationSchemes.DefaultCookieScheme);
        return Result.Success;
    }
}