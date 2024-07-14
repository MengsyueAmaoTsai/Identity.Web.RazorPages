using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;

namespace RichillCapital.Identity.Web.Pages.SignIn;

public sealed class SignInViewModel(
    IReadOnlyRepository<User> _userRepository) : 
    PageModel
{
    private static class Errors
    {
        internal const string InvalidEmailAddress = "Enter a valid email address, phone number, or Skype name.";
        internal const string UserNotFound = "We couldn't find an account with that username. Try another, or get a new Microsoft account.";
    }

    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    [Required(ErrorMessage = Errors.InvalidEmailAddress)]
    public string EmailAddress { get; init; } = string.Empty;

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            throw new ArgumentNullException("ReturnUrl is required.");
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
        var maybeUser = await _userRepository.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken);

        if (maybeUser.IsNull)
        {
            ModelState.AddModelError(Errors.UserNotFound, Errors.UserNotFound);
            return Page();
        }

        return RedirectToPage("/password/index");
    }
}