using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Users;

[Authorize]
public sealed class CreateUserViewModel(
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) :
    PageModel
{
    [Display(Name = "E-Mail")]
    [BindProperty]
    public required string Email { get; init; }

    [BindProperty]
    public required string Name { get; init; }

    [Display(Name = "Phone Number")]
    [BindProperty]
    public required string PhoneNumber { get; init; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

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

        var errorOrUser = Domain.User.Create(
            UserId.NewUserId(),
            name,
            email,
            phoneNumber,
            "123",
            lockoutEnabled: false,
            twoFactorEnabled: false,
            emailConfirmed: false,
            phoneNumberConfirmed: false,
            0,
            DateTimeOffset.UtcNow);

        if (errorOrUser.HasError)
        {
            ModelState.AddModelError(errorOrUser.Errors.First().Code, errorOrUser.Errors.First().Message);
            return Page();
        }

        var user = errorOrUser.Value;

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RedirectToPage("./index");
    }
}