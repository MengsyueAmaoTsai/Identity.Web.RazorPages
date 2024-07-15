using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;

namespace RichillCapital.Identity.Web.Pages.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    IReadOnlyRepository<User> _userRepository) :
    IdentityViewModel
{
    [BindProperty]
    [Required(ErrorMessage = "Enter a valid email address.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public required string EmailAddress { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
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

        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            ModelState.AddModelError(emailResult.Error.Code, emailResult.Error.Message);
            return Page();
        }

        var email = emailResult.Value;

        if (!await _userRepository.AnyAsync(user => user.Email == email, cancellationToken))
        {
            ModelState.AddModelError("Email", "User with this email address does not exist.");
            return Page();
        }

        return EnterPasswordPage();
    }

    private IActionResult EnterPasswordPage() => RedirectToPage(
        "/signIn/password/index",
        new
        {
            ReturnUrl,
            EmailAddress,
        });
}