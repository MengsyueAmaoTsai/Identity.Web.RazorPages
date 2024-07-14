using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Password;

public sealed class PasswordViewModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public string Password { get; init; } = string.Empty;
    
    public IActionResult OnPost()
    {
        return Page();
    }
}
