using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.EnterPassword;

public sealed class EnterPasswordViewModel :
    PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    public IActionResult OnPost()
    {
        TempData["Password"] = Password;

        return RedirectToPage("/staySignedIn/index", new
        {
            ReturnUrl,
            EmailAddress,
        });
    }
}