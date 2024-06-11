using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users;

[AllowAnonymous]
public sealed class UserDetailsViewModel() :
    PageModel
{
    [BindProperty(SupportsGet = true, Name = "userId")]
    public required string Id { get; init; }

    public required string Email { get; set; }

    public required string Name { get; set; }

    public async Task<IActionResult> OnGetAsync(
        CancellationToken cancellationToken = default)
    {
        Name = "Meng-Syue Tsai";
        Email = "test";
        return Page();
    }
}