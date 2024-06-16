using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class SignUpViewModel(
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork,
    IAuthenticationSchemeProvider _schemeProvider) :
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
        var validationResult = Result<(Email, UserName)>.Combine(
            Domain.Email.From(Email),
            UserName.From(Name));

        if (validationResult.IsFailure)
        {
            ModelState.AddModelError(validationResult.Error.Code, validationResult.Error.Message);
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var (email, name) = validationResult.Value;

        if (await _userRepository.AnyAsync(user => user.Email == email, cancellationToken))
        {
            ModelState.AddModelError(nameof(Email), "Email is already taken.");
            await InitializeAsync(cancellationToken);
            return Page();
        }

        if (Password != ConfirmPassword)
        {
            ModelState.AddModelError(nameof(ConfirmPassword), "Passwords do not match.");
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var errorOrUser = Domain.User.Create(
            UserId.NewUserId(),
            name,
            email,
            PhoneNumber.From("").Value,
            "123",
            true,
            true,
            false,
            false,
            0,
            DateTimeOffset.UtcNow);


        if (errorOrUser.HasError)
        {
            ModelState.AddModelError(errorOrUser.Errors.First().Code, errorOrUser.Errors.First().Message);
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var user = errorOrUser.Value;

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var code = "_userManager.GenerateEmailConfirmationTokenAsync(user)";

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var callbackUrl = Url.Page(
            "/identity/confirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId = user.Id.Value, code = code, returnUrl = ReturnUrl },
            protocol: Request.Scheme)!;

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