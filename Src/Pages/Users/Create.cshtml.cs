using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;

namespace RichillCapital.Identity.Web.Pages.Users;

public sealed class CreateUserViewModel(
    ILogger<CreateUserViewModel> _logger,
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) :
    PageModel
{
    [BindProperty]
    public required string Email { get; set; }

    [BindProperty]
    public required string Name { get; set; }

    [BindProperty]
    public required string PhoneNumber { get; set; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
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