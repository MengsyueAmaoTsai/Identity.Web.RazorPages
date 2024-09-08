using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.Identity.Web.Pages;
using RichillCapital.Infrastructure.Identity.Server;
using RichillCapital.SharedKernel.Monads;
using System.ComponentModel.DataAnnotations;

[AllowAnonymous]
public sealed class SignUpVerifyEmailViewModel(
    ILogger<SignUpVerifyEmailViewModel> _logger,
    IUserManager _userManager,
    ISignInManager _signInManager,
    IIdentityServerInteractionService _interactionService) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "This information is required.")]
    public required string EmailVerificationCode { get; init; }

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
            ModelState.AddModelError(
                "EmailVerificaitionCode", 
                "That code didn't work. Check the code and try again.");

            return Page();
        }

        var user = userResult.Value;

        var signInResult = await _signInManager.PasswordSignInAsync(
            email: user.Email,
            password: user.PasswordHash,
            isPersistent: false,
            lockoutOnFailure: false,
            cancellationToken);

        if (signInResult.IsFailure)
        {
            return Error();
        }

        // Redirect
        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (context is null)
        {
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            if (string.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect("~/");
            }

            throw new Exception("invalid return URL");
        }

        if (context.IsNativeClient())
        {
            return Redirecting(ReturnUrl);
        }

        return Redirect(ReturnUrl);
    }
}