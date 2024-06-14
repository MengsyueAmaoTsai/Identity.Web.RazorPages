using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Diagnostics;

[Authorize]
public class DiagnosticsViewModel : PageModel
{
}
