using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.IdentityServer;

namespace RichillCapital.Identity.Web.Pages.Clients;

[Authorize]
public sealed class ClientDetailsViewModel() :
    PageModel
{
    [BindProperty(Name = "clientId", SupportsGet = true)]
    public required string Id { get; init; }

    public required Client Client { get; set; }

    public IActionResult OnGet()
    {
        var client = InMemoryClients.Default
            .FirstOrDefault(client => client.ClientId == Id);

        if (client is null)
        {
            return NotFound();
        }

        Client = client;

        return Page();
    }
}