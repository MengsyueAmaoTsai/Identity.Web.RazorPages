using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignOut;

public sealed class SignOutViewModel : ViewModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }
}