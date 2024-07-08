using Azure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Profile.Info;

public sealed class ProfileInfoViewModel : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }
}