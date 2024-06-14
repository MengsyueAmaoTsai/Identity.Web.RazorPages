using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Models.Users;
using RichillCapital.Identity.Web.Services;

namespace RichillCapital.Identity.Web.Pages.Users;

[Authorize]
public sealed class UserDetailsViewModel(
    ILogger<UserDetailsViewModel> _logger,
    IApiService _apiService) :
    PageModel
{
    public new required UserModel User { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting user with id {UserId}", userId);

        var userResult = await _apiService.GetUserByIdAsync(userId, cancellationToken);

        if (userResult.IsFailure)
        {
            return RedirectToPage("/error");
        }

        User = userResult.Value.ToModel();

        return Page();
    }
}