using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp;

public sealed class SignUpPasswordViewModel : IdentityViewModel
{
    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            throw new ArgumentNullException(nameof(ReturnUrl));
        }

        if (string.IsNullOrEmpty(EmailAddress))
        {
            throw new ArgumentNullException(nameof(EmailAddress));
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        TempData["Password"] = Password;

        return RedirectToCreateProfilePage();
    }

    private IActionResult RedirectToCreateProfilePage()
    {
        return RedirectToPage(
            "/signUp/birthDate/index",
            new 
            {
                ReturnUrl,
                EmailAddress,
            });
    }
}