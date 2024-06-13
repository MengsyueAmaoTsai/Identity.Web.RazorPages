using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Services;

namespace RichillCapital.Identity.Web.Pages.Users;

[AllowAnonymous]
public sealed class UserDetailsViewModel(
    IApiService _apiService) :
    PageModel
{
    [BindProperty(SupportsGet = true, Name = "userId")]
    public required string Id { get; init; }

    public required string Email { get; set; }

    public required string Name { get; set; }

    public async Task<IActionResult> OnGetAsync(
        CancellationToken cancellationToken = default)
    {
        var user = await _apiService.GetUsersByIdAsync(Id, cancellationToken);

        if (user.IsFailure)
        {
            return NotFound();
        }

        Email = user.Value.Email;
        Name = user.Value.Name;

        return Page();
    }
}