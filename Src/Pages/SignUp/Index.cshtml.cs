using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;

namespace RichillCapital.Identity.Web.Pages.SignUp;

public sealed class SignUpViewModel(
    IReadOnlyRepository<User> _userRepository) : 
    IdentityViewModel
{
    private static class Errors
    {
        internal const string EmailRequired = "";
        internal const string UserNotFound = "";
    }

    [BindProperty]
    [Required(ErrorMessage = Errors.EmailRequired)]
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
                Errors.UserNotFound);

            return Page();
        }
        
        return RedirectToPage("/signUp/password", new
        {
            ReturnUrl,
            EmailAddress,
        });
    }
}