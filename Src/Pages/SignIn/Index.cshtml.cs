using System.ComponentModel.DataAnnotations;
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
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [Required(ErrorMessage = "Enter a valid email address.")]
    public required string EmailAddress { get; init; }

    public IActionResult OnGet()
    {
        _logger.LogInformation(
            "Sign in page loaded with ReturnUrl: {returnUrl}",
            ReturnUrl);

        return Page();
    }

    public IActionResult OnPost()
    {
        _logger.LogInformation("Processing sign-in for ReturnUrl: {ReturnUrl}", ReturnUrl);

        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            _logger.LogWarning("Sign-in failed due to invalid email format. Email: {EmailAddress}", EmailAddress);
            return Page();
        }

        var email = emailResult.Value;
        _logger.LogInformation("Sign-in succeeded for Email: {Email}, redirecting to password entry.", email);
        return SignInPassword(ReturnUrl, email);
    }

    public IActionResult OnPostBack()
    {
        _logger.LogInformation("User clicked back on the sign-in page.");
        return Page();
    }
}