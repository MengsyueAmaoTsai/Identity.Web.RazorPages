using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users;

[AllowAnonymous]
public sealed class UsersViewModel() :
    PageModel
{
    public required IEnumerable<UserModel> Users { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        Users = [
            new UserModel
            {
                Id = "1",
                Email = "mengsyue.tsai@outlook.com",
                Name = "Meng-Syue Tsai",
            }
        ];
        return Page();
    }
}

public sealed record UserModel
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
}