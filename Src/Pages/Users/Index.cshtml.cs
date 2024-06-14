using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Models.Users;
using RichillCapital.Identity.Web.Services;

namespace RichillCapital.Identity.Web.Pages.Users;

public sealed class ListUsersViewModel(
    IApiService _apiService) : PageModel
{
    public required IEnumerable<UserModel> Users { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var users = await _apiService.ListUsersAsync();

        if (users.IsFailure)
        {
            return RedirectToPage("/Error");
        }

        Users = users.Value.Items
            .Select(user => user.ToModel());

        return Page();
    }
}
