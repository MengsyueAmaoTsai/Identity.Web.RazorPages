using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Clients;

[Authorize]
public sealed class ClientDetailsViewModel() :
    PageModel
{
}