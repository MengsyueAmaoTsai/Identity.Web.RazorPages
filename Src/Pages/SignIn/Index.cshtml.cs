using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;

namespace RichillCapital.Identity.Web.Pages.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger,
    IUserManager _userManager) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "Enter a valid email address.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public required string EmailAddress { get; init; }

    public IActionResult OnGet()
    {
        _logger.LogInformation(
            "Sign in page loaded with ReturnUrl: {returnUrl}",
            ReturnUrl);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing sign-in for ReturnUrl: {ReturnUrl}", ReturnUrl);

        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            _logger.LogWarning(
                "Sign-in failed due to invalid email format. Email: {EmailAddress}",
                EmailAddress);

            ModelState.AddModelError(nameof(EmailAddress), emailResult.Error.Message);
            return Page();
        }

        var email = emailResult.Value;

        var userResult = await _userManager.FindByEmailAsync(email, cancellationToken);

        if (userResult.IsFailure)
        {
            _logger.LogWarning(
                "Sign-in failed because the email does not exist. Email: {EmailAddress}",
                EmailAddress);

            ModelState.AddModelError(
                nameof(EmailAddress),
                "We couldn't find an account with that username. Try another, or get a new Microsoft account.");

            return Page();
        }

        _logger.LogInformation(
            "Sign-in succeeded for Email: {Email}, redirecting to password entry.",
            email);

        return SignInPassword(ReturnUrl, email);
    }

    public IActionResult OnPostBack()
    {
        _logger.LogInformation("User clicked back on the sign-in page.");
        return Page();
    }
}