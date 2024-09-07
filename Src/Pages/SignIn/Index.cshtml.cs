using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;

namespace RichillCapital.Identity.Web.Pages.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    public required string EmailAddress { get; init; }

    public IActionResult OnGet()
    {
        _logger.LogInformation("ReturnUrl: {returnUrl}", ReturnUrl);
        return Page();
    }

    public IActionResult OnPost()
    {
        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            _logger.LogError("{error}", emailResult.Error);
            return Page();
        }

        var email = emailResult.Value;
        _logger.LogInformation("ReturnUrl: {returnUrl}, Email: {email}", ReturnUrl, email);
        return SignInPassword(ReturnUrl, email);
    }

    public IActionResult OnPostBack()
    {
        _logger.LogInformation("Back");
        return Page();
    }
}