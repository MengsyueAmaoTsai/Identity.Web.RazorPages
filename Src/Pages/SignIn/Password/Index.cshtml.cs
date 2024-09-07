using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignIn.Password;

[AllowAnonymous]
public sealed class SignInPasswordViewModel(
    ILogger<SignInPasswordViewModel> _logger) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    public IActionResult OnPostAsync()
    {
        TempData["Password"] = Password;
        return SignInStaySignedIn(ReturnUrl, EmailAddress);
    }
}