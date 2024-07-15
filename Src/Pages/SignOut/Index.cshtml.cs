using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignOut;

[Authorize]
public sealed class SignOutViewModel : ViewModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        return SignedOutPage();
    }

    private IActionResult SignedOutPage() => RedirectToPage("/signOut/signedOut/index");
}