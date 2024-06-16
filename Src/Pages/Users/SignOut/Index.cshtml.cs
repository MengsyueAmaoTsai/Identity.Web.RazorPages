using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Users.SignOut;

[Authorize]
public sealed class SignOutViewModel(
    ILogger<SignOutViewModel> _logger,
    ICurrentUser _currentUser) : 
    PageModel
{
    public async Task<IActionResult> OnPostAsync(
        CancellationToken _ = default)
    {
        await HttpContext.SignOutAsync(RichillCapitalAuthenticationSchemes.DefaultCookieScheme);

        _logger.LogInformation("User {userId} signed out", _currentUser.Id);

        return RedirectToPage("/users/signed-out");
    }
}