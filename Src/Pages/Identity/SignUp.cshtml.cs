using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
[ValidateAntiForgeryToken]
public sealed class SignUpViewModel(
    IAuthenticationSchemeProvider _schemeProvider,
    IReadOnlyRepository<User> _userRepository,
    IUserManager _userManager) :
    PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    public required string Name { get; init; }

    [BindProperty]
    public required string Email { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    [BindProperty]
    public required string ConfirmPassword { get; init; }

    public required IEnumerable<AuthenticationScheme> ExternalSchemes { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        await InitializeAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var validationResult = Result<(UserName, Email)>.Combine(
            UserName.From(Name),
            Domain.Users.Email.From(Email));

        if (validationResult.IsFailure)
        {
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var (name, email) = validationResult.Value;

        if (await _userRepository.AnyAsync(user => user.Email == email, cancellationToken))
        {
            ModelState.AddModelError(nameof(Email), "Email is already taken.");
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var errorOrUser = Domain.Users.User.Create(
            id: UserId.NewUserId(),
            name: name,
            email: email,
            emailConfirmed: false,
            passwordHash: Password,
            lockoutEnabled: true,
            accessFailedCount: 0,
            createdAt: DateTimeOffset.UtcNow);

        if (errorOrUser.HasError)
        {
            ModelState.AddModelError(errorOrUser.Errors.First().Code, errorOrUser.Errors.First().Message);
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var user = errorOrUser.Value;

        var result = await _userManager.CreateAsync(user, Password);

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.Error.Code, result.Error.Message);
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user, cancellationToken);
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var callbackUrl = Url.Page(
            "/identity/confirmEmail",
            pageHandler: null,
            values: new 
            { 
                Area = "Identity", 
                UserId = user.Id.Value, 
                Code = code,
                ReturnUrl,
            },
            protocol: Request.Scheme) ?? string.Empty;

        // await _emailSender.SendConfirmationLinkAsync(user, user.Email.Value, HtmlEncoder.Default.Encode(callbackUrl));

        // if (_userManager.Options.SignIn.RequireConfirmedAccount)
        // {
        //     return RedirectToPage("RegisterConfirmation", new { email = user.Email.Value, returnUrl = ReturnUrl });
        // }

        // await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(ReturnUrl);
    }

    private async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        ExternalSchemes = await _schemeProvider.GetExternalSchemesAsync();
    }
}