using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp.Birthdate;

[AllowAnonymous]
public sealed class SignUpBirthdateViewModel(
    ILogger<SignUpBirthdateViewModel> _logger) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    public IActionResult OnPost()
    {
        return SignUpVerifyEmail();
    }
}