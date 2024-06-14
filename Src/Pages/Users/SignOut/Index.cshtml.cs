using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users.SignOut;

[Authorize]
public sealed class SignOutViewModel() : PageModel
{
    public async Task<IActionResult> OnPostAsync(
        CancellationToken _ = default)
    {
        // await HttpContext.SignOutAsync(RichillCapitalAuthenticationSchemes.Default);
        await HttpContext.SignOutAsync(RichillCapitalAuthenticationSchemes.Cookie);

        return RedirectToPage("/users/signedout");
    }
}