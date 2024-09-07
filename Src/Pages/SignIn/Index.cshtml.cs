using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;

namespace RichillCapital.Identity.Web.Pages.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger) :
    ViewModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    public required string EmailAddress { get; init; }

    public IActionResult OnPost()
    {
        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            _logger.LogError("{error}", emailResult.Error);
            return Page();
        }

        return SignInPassword();
    }
}