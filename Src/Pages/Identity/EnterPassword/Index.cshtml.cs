using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity.EnterPassword;

public class EnterPasswordViewModel() :
    PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        TempData["Password"] = Password;

        return RedirectToPage(
            "/identity/staySignedIn/index",
            new
            {
                ReturnUrl,
                EmailAddress,
            });
    }
}
