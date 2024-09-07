using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RichillCapital.Identity.Web.Pages.SignUp.CreatePassword;

public sealed class SignUpCreatePasswordViewModel(
    ILogger<SignUpCreatePasswordViewModel> _logger) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "A password is required")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Passwords must have at least 8 characters and contain at least two of the following: uppercase letters, lowercase letters, numbers, and symbols.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Passwords must have at least 8 characters and contain at least two of the following: uppercase letters, lowercase letters, numbers, and symbols.")]
    public required string Password { get; init; }

    public IActionResult OnPost()
    {
        TempData.Add("CreatePassword", Password);
        return Page();
    }
}