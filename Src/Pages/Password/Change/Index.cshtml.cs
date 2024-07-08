using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Password.Change;

public sealed class PasswordChangeViewModel : PageModel
{
    [BindProperty]
    public required string CurrentPassword { get; init; }

    [BindProperty]
    public required string NewPassword { get; init; }

    [BindProperty]
    public required string ReenterPassword { get; init; }

    public async Task<IActionResult> OnPostAsync()
    {
        return Page();
    }
}