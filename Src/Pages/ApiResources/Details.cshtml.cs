using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.ApiResources;

public sealed class ApiResourceDetailsViewModel : PageModel
{
    public required ApiResource ApiResource { get; set; }
    public IActionResult OnGet(string resourceId)
    {
        var resource = InMemoryApiResources.Default
            .FirstOrDefault(resource => resource.Name == resourceId);

        if (resource is null)
        {
            return NotFound();
        }

        ApiResource = resource;
        return Page();
    }
}