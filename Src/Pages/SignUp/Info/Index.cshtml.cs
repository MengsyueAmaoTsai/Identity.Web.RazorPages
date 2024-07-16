using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.SignUp.Info;

[AllowAnonymous]
public sealed class SignUpInfoViewModel(
    IUserManager _userManager,
    ISignInManager _signInManager) :
    IdentityViewModel
{
    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

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

    public async Task<IActionResult> OnPostAsync(
        CancellationToken cancellationToken = default)
    {
        var name = UserName.From("IdentityWeb").ThrowIfFailure().ValueOrDefault;
        var email = Email.From(EmailAddress).ThrowIfFailure().ValueOrDefault;
        var password = TempData["Password"]?.ToString() ??
            throw new InvalidOperationException("Password not found in TempData.");

        var user = Domain.Users.User
            .Create(
                UserId.NewUserId(),
                name,
                email,
                emailConfirmed: true,
                password,
                lockoutEnabled: true,
                accessFailedCount: 0,
                createdAt: DateTimeOffset.UtcNow)
            .ThrowIfError()
            .ValueOrDefault;

        var result = await _userManager.CreateAsync(user, password, cancellationToken);

        if (result.IsFailure)
        {
            return ErrorPage();
        }

        var signInResult = await _signInManager.PasswordSignInAsync(
            email,
            password,
            isPersistent: true,
            lockoutOnFailure: true,
            cancellationToken);

        if (signInResult.IsFailure)
        {
            return ErrorPage();
        }

        return IndexPage();
    }
}