using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp.Password;

public sealed class SignUpPasswordViewModel :
    IdentityViewModel
{
    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "A password is required")]
    public required string Password { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            return ErrorPage();
        }

        if (string.IsNullOrEmpty(EmailAddress))
        {
            return ErrorPage();
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Password.Length < 8)
        {
            ModelState.AddModelError(
                "Password",
                "Passwords must have at least 8 characters and contain at least two of the following: uppercase letters, lowercase letters, numbers, and symbols.");

            return Page();
        }

        TempData["Password"] = Password;

        return InfoPage();
    }

    private IActionResult InfoPage() => RedirectToPage(
        "/signUp/info/index",
        new
        {
            ReturnUrl,
            EmailAddress,
        });
}