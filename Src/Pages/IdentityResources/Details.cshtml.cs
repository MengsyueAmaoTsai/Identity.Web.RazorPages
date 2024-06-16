using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.IdentityResources;

public sealed class IdentityResourceDetailsViewModel : PageModel
{
    public required IdentityResource IdentityResource { get; set; }

    public IActionResult OnGet(string resourceId)
    {
        var resource = InMemoryIdentityResources.Default
            .FirstOrDefault(resource => resource.Name == resourceId);

        if (resource is null)
        {
            return NotFound();
        }

        IdentityResource = resource;

        return Page();
    }
}