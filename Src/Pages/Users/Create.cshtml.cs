using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;

namespace RichillCapital.Identity.Web.Pages.Users;

[AllowAnonymous]
public sealed class CreateUserViewModel(
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) : PageModel
{
    public required string Email { get; init; }

    public required string Name { get; init; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var errorOrUser = Domain.User.Create(
            UserId.NewUserId(),
            Domain.Email.From("test@gmail.com").Value,
            "123",
            "Test User");

        if (errorOrUser.HasError)
        {
            var error = errorOrUser.Errors.First();

            ModelState.AddModelError(error.Code, error.Message);

            return Page();
        }

        var user = errorOrUser.Value;

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RedirectToPage("./Index");
    }
}