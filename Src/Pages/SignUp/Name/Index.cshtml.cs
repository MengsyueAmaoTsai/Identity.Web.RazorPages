using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp.Name;

[AllowAnonymous]
public sealed class SignUpNameViewModel(
    ILogger<SignUpNameViewModel> _logger) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "This information is required.")]
    public required string Name { get; init; }

    public IActionResult OnPost()
    {
        _logger.LogInformation(
            "User {EmailAddress} has entered their name as {Name}.",
            EmailAddress,
            Name);

        return SignUpBirthdate(ReturnUrl, EmailAddress, Name);
    }
}