using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.Identity.Web.Pages;
using RichillCapital.SharedKernel.Monads;

[AllowAnonymous]
public sealed class SignUpVerifyEmailViewModel(
    ILogger<SignUpVerifyEmailViewModel> _logger,
    IUserManager _userManager,
    ISignInManager _signInManager) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty(Name = "name", SupportsGet = true)]
    public required string Name { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "This information is required.")]
    public required string EmailVerificationCode { get; init; }

    public async Task<IActionResult> OnGetAsync()
    {
        // Generate email confirmation code and send email to user
        var errorOrUser = RichillCapital.Domain.User.Create(
            UserId.NewUserId(),
            Name,
            Email.From(EmailAddress).ThrowIfFailure().Value,
            false,
            TempData["Password"] as string ?? string.Empty);

        if (errorOrUser.HasError)
        {
            return Error();
        }

        var user = errorOrUser.Value;

        await _userManager.CreateAsync(user, TempData["Password"] as string ?? string.Empty);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        _logger.LogInformation("Generate confirmation code: {code}", code);

        // Send email to user
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        // if invalid : That code didn't work. Check the code and try again.

        var email = Email.From(EmailAddress).ThrowIfFailure().Value;

        var userResult = await _userManager.FindByEmailAsync(email, cancellationToken).ThrowIfFailure();

        var confirmResult = await _userManager.ConfirmEmailAsync(
            userResult.Value,
            EmailVerificationCode);

        if (confirmResult.IsFailure)
        {
            ModelState.AddModelError(string.Empty, "That code didn't work. Check the code and try again.");

            return Page();
        }

        // Sign in user

        return Page();
    }
}