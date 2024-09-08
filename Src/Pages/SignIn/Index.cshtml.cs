using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.Infrastructure.Identity.Server;
using System.ComponentModel.DataAnnotations;

namespace RichillCapital.Identity.Web.Pages.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger,
    IUserManager _userManager,
    IIdentityServerInteractionService _interactionService) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "Enter a valid email address.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public required string EmailAddress { get; init; }

    private bool ExternalSignInOnly => false;

    public IActionResult OnGet()
    {
        if (ExternalSignInOnly)
        {
            return RedirectToPage("/signIn/signInOptions/index");
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            ModelState.AddModelError(nameof(EmailAddress), emailResult.Error.Message);
            return Page();
        }

        var email = emailResult.Value;

        var userResult = await _userManager.FindByEmailAsync(email, cancellationToken);

        if (userResult.IsFailure)
        {
            ModelState.AddModelError(
                nameof(EmailAddress),
                "We couldn't find an account with that username. Try another, or get a new Microsoft account.");

            return Page();
        }

        return SignInPassword(ReturnUrl, email);
    }

    public async Task<IActionResult> OnPostBackAsync()
    {
        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (context is null)
        {
            return Redirect("~/");
        }

        await _interactionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

        if (context.IsNativeClient())
        {
            return Redirecting(ReturnUrl);
        }

        return Redirect(ReturnUrl);
    }
}