using System.ComponentModel.DataAnnotations;
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
    [Required(ErrorMessage = "Please enter the password for your Microsoft account.")]
    public required string Password { get; init; }

    public IActionResult OnPostAsync()
    {
        // invalid password: Your account or password is incorrect. If you don't remember your password, <a>reset it now.</a>
        TempData["Password"] = Password;
        return SignInStaySignedIn(ReturnUrl, EmailAddress);
    }
}