using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.IdentityServer;

namespace RichillCapital.Identity.Web.Pages.Clients;

[Authorize]
public sealed class ListClientsViewModel : PageModel
{
    public required IEnumerable<Client> Clients { get; set; } = [];

    public IActionResult OnGet()
    {
        Clients = InMemoryClients.Default;

        return Page();
    }
}
