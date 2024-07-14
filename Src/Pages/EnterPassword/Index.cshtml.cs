using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.EnterPassword;

public sealed class EnterPasswordViewModel :
    PageModel
{
    private static class Errors
    {
        internal const string PasswordRequired = "Please enter the password for your Richill Capital account.";
        internal const string IncorrectPassword = "Your account or password is incorrect. If you don't remember your password, <a>reset it now.</a>";
    }

    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    [Required(ErrorMessage = Errors.PasswordRequired)]
    public required string Password { get; init; }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        TempData["Password"] = Password;

        return RedirectToPage("/staySignedIn/index", new
        {
            ReturnUrl,
            EmailAddress,
        });
    }
}