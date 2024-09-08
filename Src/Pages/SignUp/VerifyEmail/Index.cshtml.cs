using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Identity.Web.Pages;

[AllowAnonymous]
public sealed class SignUpVerifyEmailViewModel(
    ILogger<SignUpVerifyEmailViewModel> _logger) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty(Name = "name", SupportsGet = true)]
    public required string Name { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "This information is required.")]
    public required string EmailVerificationCode { get; init; }

    public IActionResult OnPost()
    {
        // if invalid : That code didn't work. Check the code and try again.
        return Page();
    }
}