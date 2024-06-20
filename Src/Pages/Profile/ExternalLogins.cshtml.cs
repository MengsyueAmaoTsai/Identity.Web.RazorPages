using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Profile;

[Authorize]
public sealed class ExternalLoginsViewModel : PageModel
{
}