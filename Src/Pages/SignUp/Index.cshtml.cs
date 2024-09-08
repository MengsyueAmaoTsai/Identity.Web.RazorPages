using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;

namespace RichillCapital.Identity.Web.Pages.SignUp;

[AllowAnonymous]
public sealed class SignUpViewModel(
    ILogger<SignUpViewModel> _logger,
    IUserManager _userManager) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "An email address is required.")]
    [EmailAddress(ErrorMessage = "Enter the email address in the format someone@example.com.")]
    public required string EmailAddress { get; init; }

    public async Task<IActionResult> OnPostAsync(
        CancellationToken cancellationToken = default)
    {
        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            _logger.LogWarning("{error}", emailResult.Error);
            ModelState.AddModelError("EmailAddress", emailResult.Error.Message);
            return Page();
        }

        var email = emailResult.Value;

        var userResult = await _userManager.FindByEmailAsync(email, cancellationToken);

        if (userResult.IsSuccess)
        {
            _logger.LogWarning(
                "Sign-up failed because the email already exists. Email: {EmailAddress}",
                email);

            ModelState.AddModelError(
                "EmailAddress",
                $"{email} is already a Microsoft account. Try another name or claim one of these that's available. If it's yours, sign in now.");

            return Page();
        }

        return SignUpCreatePassword(ReturnUrl, email);
    }
}