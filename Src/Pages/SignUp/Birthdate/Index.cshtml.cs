using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.SignUp.Birthdate;

[AllowAnonymous]
public sealed class SignUpBirthdateViewModel(
    ILogger<SignUpBirthdateViewModel> _logger,
    IUserManager _userManager) :
    ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "emailAddress", SupportsGet = true)]
    public required string EmailAddress { get; init; }

    public async Task<IActionResult> OnPostAsync()
    {
        var password = TempData["Password"] as string ?? string.Empty;

        var errorOrUser = RichillCapital.Domain.User.Create(
            UserId.NewUserId(),
            string.Empty,
            Email.From(EmailAddress).ThrowIfFailure().Value,
            false,
            password);

        if (errorOrUser.HasError)
        {
            return Error();
        }

        var user = errorOrUser.Value;

        var craeteResult = await _userManager.CreateAsync(user, password);

        if (craeteResult.IsFailure)
        {
            return Error();
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        _logger.LogInformation("Send email confirmation code: {code} to {email}", code, user.Email);

        return SignUpVerifyEmail();
    }
}