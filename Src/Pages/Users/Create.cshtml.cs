using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users.Create;

public sealed class CreateUserViewModel() : PageModel
{
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        return Page();
    }
}