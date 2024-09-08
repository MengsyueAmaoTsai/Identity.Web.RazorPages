using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.SharedKernel.Monads;
using System.ComponentModel.DataAnnotations;

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
        var checkPasswordResult = await VerifyPasswordAsync(EmailAddress, Password);

        if (checkPasswordResult.IsFailure)
        {
            _logger.LogWarning("{error}", checkPasswordResult.Error);

            ModelState.AddModelError(
                nameof(Password),
                "Your account or password is incorrect. If you don't remember your password, reset it now.");

            return Page();
        }

        TempData["Password"] = Password;
        return SignInStaySignedIn(ReturnUrl, EmailAddress);
    }

    private async Task<Result> VerifyPasswordAsync(
        string emailAddress, 
        string password,
        CancellationToken cancellationToken = default)
    {
        var emailResult = Email.From(emailAddress);

        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        var email = emailResult.Value;

        var userResult = await _userManager.FindByEmailAsync(email);

        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Error);
        }

        var user = userResult.Value;

        return _userManager.CheckPassword(user, password);
    }
}