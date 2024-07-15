using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.SignIn.StaySignedIn;

[AllowAnonymous]
public sealed class StaySignedInViewModel(
    ISignInManager _signInManager) :
    IdentityViewModel
{
    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required bool DoNotShowThisAgain { get; init; }

    [BindProperty]
    public required bool StaySignedIn { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            return ErrorPage();
        }

        if (string.IsNullOrEmpty(EmailAddress))
        {
            return ErrorPage();
        }

        if (TempData.TryGetValue("Password", out _))
        {
            TempData.Keep("Password");
        }
        else
        {
            return ErrorPage();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var email = Email.From(EmailAddress).ThrowIfFailure().ValueOrDefault;

        var password = TempData["Password"]?.ToString() ??
            throw new InvalidOperationException("Password not found in TempData.");

        var signInResult = await _signInManager.PasswordSignInAsync(
            email,
            password,
            StaySignedIn,
            lockoutOnFailure: true,
            cancellationToken);

        if (signInResult.IsFailure)
        {
            return ErrorPage();
        }

        return ProfilePage();
    }
}