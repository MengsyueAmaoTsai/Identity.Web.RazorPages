using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Services;

namespace RichillCapital.Identity.Web.Pages.Users;

[AllowAnonymous]
public sealed class UsersViewModel(
    IApiService _apiService) :
    PageModel
{
    public required IEnumerable<UserModel> Users { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var users = await _apiService.ListUsersAsync(cancellationToken);

        if (users.IsFailure)
        {
            return RedirectToPage("/Error");
        }

        Users = users.Value.Items
            .Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name
            });

        return Page();
    }
}

public sealed record UserModel
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
}