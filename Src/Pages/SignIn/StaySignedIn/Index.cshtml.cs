using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.Infrastructure.Identity.Server;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.SignIn.StaySignedIn;

[AllowAnonymous]
public sealed class SignInStaySignedInViewModel(
    ILogger<SignInStaySignedInViewModel> _logger,
    ISignInManager _signInManager,
    IIdentityServerInteractionService _interactionService) :
    ViewModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required bool DoNotShowThisAgain { get; init; }

    [BindProperty]
    public required bool StaySignedIn { get; init; }

    public async Task<IActionResult> OnPostAsync(
        CancellationToken cancellationToken = default)
    {
        var password = TempData["Password"]?.ToString() ?? string.Empty;

        var signInResult = await _signInManager.PasswordSignInAsync(
            email: Email.From(EmailAddress).ThrowIfFailure().Value,
            password: password,
            isPersistent: StaySignedIn,
            lockoutOnFailure: true,
            cancellationToken);

        if (signInResult.IsFailure)
        {
            _logger.LogWarning("{error}", signInResult.Error);
            
            ModelState.AddModelError(string.Empty, signInResult.Error.Message);
            return Page();
        }

        TempData.Remove("Password");

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