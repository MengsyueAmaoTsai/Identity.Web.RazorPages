using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.IdentityServer;

namespace RichillCapital.Identity.Web.Pages.Clients;

[Authorize]
public sealed class ClientDetailsViewModel : 
    PageModel
{
    public required Client Client { get; set; }

    public IActionResult OnGet(string clientId)
    {
        var client = InMemoryClients.Default
            .FirstOrDefault(x => x.ClientId == clientId);

        if (client is null)
        {
            return NotFound();
        }

        Client = client;

        return Page();
    }
}
