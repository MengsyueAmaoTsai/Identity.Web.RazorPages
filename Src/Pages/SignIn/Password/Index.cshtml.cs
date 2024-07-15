using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.SignIn.Password;

[AllowAnonymous]
public sealed class SignInPasswordViewModel(
    IReadOnlyRepository<User> _userRepository) :
    IdentityViewModel
{
    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter the password for your Microsoft account.")]
    public required string Password { get; init; }

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

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var email = Email.From(EmailAddress).ThrowIfFailure().ValueOrDefault;

        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken)
            .ThrowIfNull();

        var user = maybeUser.ValueOrDefault;

        if (Password != user.PasswordHash)
        {
            ModelState.AddModelError("Password", "Your account or password is incorrect. If you don't remember your password, <a>reset it now.</a>");
            return Page();
        }

        TempData["Password"] = Password;

        return StaySignedInPage();
    }

    private IActionResult StaySignedInPage() => RedirectToPage(
        "/signIn/staySignedIn/index",
        new
        {
            ReturnUrl,
            EmailAddress,
        });
}