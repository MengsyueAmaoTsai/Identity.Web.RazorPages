using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;

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

        var emailResult = Domain.Email.From(Email);

        if (emailResult.IsFailure)
        {
            ModelState.AddModelError(emailResult.Error.Code, emailResult.Error.Message);

            return Page();
        }

        var errorOrUser = Domain.User.Create(UserId.NewUserId(), emailResult.Value, "123", Name);

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