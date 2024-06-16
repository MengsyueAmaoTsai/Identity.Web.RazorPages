using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.ApiResources;

public sealed class ListApiResourcesViewModel : PageModel
{
    public required IEnumerable<ApiResource> ApiResources { get; set; } = [];

    public IActionResult OnGet()
    {
        ApiResources = InMemoryApiResources.Default;
        return Page();
    }
}