using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RichillCapital.Identity.Web.Pages.SignIn;

public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger) : PageModel
{
    private static class Errors
    {
        internal const string InvalidEmailAddress = "Enter a valid email address, phone number, or Skype name.";
    }

    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    [Required(ErrorMessage = Errors.InvalidEmailAddress)]
    [EmailAddress(ErrorMessage = Errors.InvalidEmailAddress)]
    public required string EmailAddress { get; init; }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            throw new ArgumentNullException("ReturnUrl is required.");
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("\nModel state is not valid");

            foreach (var state in ModelState.Values)
            {
                _logger.LogInformation("\nValidationState: {state}", state.ValidationState);

                foreach (var error in state.Errors)
                {
                    _logger.LogError("\nError. \n\tException: {exception}\n\tErrorMessage: {message}", error.Exception, error.ErrorMessage);
                }
            }

            return Page();
        }

        _logger.LogInformation("User signed in with email address: {EmailAddress}", EmailAddress);

        return Page();
    }
}