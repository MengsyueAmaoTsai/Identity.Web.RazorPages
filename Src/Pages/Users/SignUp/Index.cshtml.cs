using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Users.SignUp;

[AllowAnonymous]
public sealed class SignUpViewModel(
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) :
    PageModel
{
    [BindProperty]
    public required string Email { get; set; }

    [BindProperty]
    public required string Password { get; set; }

    [BindProperty]
    public required string ConfirmPassword { get; set; }

    [BindProperty]
    public required string Name { get; set; }

    [BindProperty]
    public required string PhoneNumber { get; set; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var result = Result<(Domain.Email, UserName, Domain.PhoneNumber)>.Combine(
            Domain.Email.From(Email),
            UserName.From(Name),
            Domain.PhoneNumber.From(PhoneNumber));

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.Error.Code, result.Error.Message);

            return Page();
        }

        var (email, name, phoneNumber) = result.Value;

        if (!(Password == ConfirmPassword))
        {
            ModelState.AddModelError("PasswordMismatch", "Passwords do not match");

            return Page();
        }

        var maybeUser = await _userRepository.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (maybeUser.HasValue)
        {
            ModelState.AddModelError("UserExists", "User with this email already exists");

            return Page();
        }

        var errorOrUser = Domain.User.Create(
            UserId.NewUserId(),
            name,
            email,
            phoneNumber,
            Password,
            lockoutEnabled: false,
            twoFactorEnabled: false,
            emailConfirmed: false,
            phoneNumberConfirmed: false,
            0,
            DateTimeOffset.UtcNow);

        if (errorOrUser.HasError)
        {
            var error = errorOrUser.Errors.First();
            ModelState.AddModelError(error.Code, error.Message);
            return Page();
        }

        var user = errorOrUser.Value;
        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RedirectToPage("/users/signin/index");
    }
}
