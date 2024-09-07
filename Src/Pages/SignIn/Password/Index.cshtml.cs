using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.SignIn.Password;

[AllowAnonymous]
public sealed class SignInPasswordViewModel(
    ILogger<SignInPasswordViewModel> _logger,
    IUserManager _userManager) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter the password for your Microsoft account.")]
    public required string Password { get; init; }

    public async Task<IActionResult> OnPostAsync(
        CancellationToken _ = default)
    {
        var email = Email.From(EmailAddress).ThrowIfFailure().Value;

        var userResult = await _userManager.FindByEmailAsync(email);
        var user = userResult.ThrowIfFailure().Value;

        var checkPasswordResult = _userManager.CheckPassword(user, Password);

        if (checkPasswordResult.IsFailure)
        {
            _logger.LogWarning(
                "Failed to check password for user with email address {EmailAddress}. {error}",
                email,
                checkPasswordResult.Error);

            ModelState.AddModelError(
                nameof(Password),
                "Your account or password is incorrect. If you don't remember your password, reset it now.");

            return Page();
        }

        TempData["Password"] = Password;
        return SignInStaySignedIn(ReturnUrl, EmailAddress);
    }
}