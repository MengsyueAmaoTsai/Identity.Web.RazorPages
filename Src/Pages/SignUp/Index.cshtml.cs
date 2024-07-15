using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;

namespace RichillCapital.Identity.Web.Pages.SignUp;

public sealed class SignUpViewModel(
    ILogger<SignUpViewModel> _logger,
    IReadOnlyRepository<User> _userRepository) :
    IdentityViewModel
{
    [BindProperty]
    [Required(ErrorMessage = "An email address is required")]
    [EmailAddress(ErrorMessage = "Enter the email address in the format someone@example.com.")]
    public required string EmailAddress { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            throw new ArgumentNullException(nameof(ReturnUrl));
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Model state is invalid.");
            return Page();
        }

        var emailResult = Email.From(EmailAddress);

        if (emailResult.IsFailure)
        {
            ModelState.AddModelError(
                emailResult.Error.Code,
                emailResult.Error.Message);

            return Page();
        }


        var email = emailResult.Value;

        var maybeUser = await _userRepository.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken);

        if (maybeUser.HasValue)
        {
            ModelState.AddModelError(
                "Conflict",
                "Someone already has this email address. Try another name or <a>claim one of these that's available</a>");

            _logger.LogError("Email already used.");
            return Page();
        }

        return RedirectToCreatePasswordPage();
    }

    private IActionResult RedirectToCreatePasswordPage() => RedirectToPage(
        "/signUp/password/index",
        new
        {
            ReturnUrl,
            EmailAddress,
        });
}