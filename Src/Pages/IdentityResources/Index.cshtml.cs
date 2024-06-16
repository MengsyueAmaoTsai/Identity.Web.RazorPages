using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace RichillCapital.Identity.Web.Pages.IdentityResources;

public sealed class ListIdentityResourcesViewModel : PageModel
{
    public required IEnumerable<IdentityResource> IdentityResources { get; set; } = [];

    public IActionResult OnGet()
    {
        IdentityResources = InMemoryIdentityResources.Default;

        return Page();
    }
}