using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp;

public sealed class SignUpBirthdayViewModel : IdentityViewModel
{
    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required string Country { get; init; }

    [BindProperty]
    public required int Year { get; init; }

    [BindProperty]
    public required int Month { get; init; }

    [BindProperty]
    public required int Day { get; init; }

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
        return RedirectToPage("/signUp/notice", new
        {
            ReturnUrl,
        });
    }
}