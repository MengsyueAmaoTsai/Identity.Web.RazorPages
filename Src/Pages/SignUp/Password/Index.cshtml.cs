using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp;

public sealed class SignUpPasswordViewModel : IdentityViewModel
{
    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            throw new ArgumentNullException(nameof(ReturnUrl));
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("/signUp/birthday", new
        {
            ReturnUrl,
        });
    }
}