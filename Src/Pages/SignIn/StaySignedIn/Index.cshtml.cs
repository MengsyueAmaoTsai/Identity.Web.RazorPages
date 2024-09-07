using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignIn.StaySignedIn;

[AllowAnonymous]
public sealed class SignInStaySignedInViewModel(
    ILogger<SignInStaySignedInViewModel> _logger) :
    ViewModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required bool DoNotShowThisAgain { get; init; }

    public required bool StaySignedIn { get; init; }

    public IActionResult OnPost()
    {
        return Page();
    }
}