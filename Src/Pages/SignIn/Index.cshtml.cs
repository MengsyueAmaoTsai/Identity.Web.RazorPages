using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.SignIn;

public sealed class SignInViewModel : PageModel
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
}