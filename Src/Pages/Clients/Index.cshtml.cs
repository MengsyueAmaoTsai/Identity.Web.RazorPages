using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.IdentityServer;

namespace RichillCapital.Identity.Web.Pages.Clients;

[Authorize]
public sealed class ClientsViewModel() :
    PageModel
{
    public required IEnumerable<Client> Clients = InMemoryClients.Default;
}