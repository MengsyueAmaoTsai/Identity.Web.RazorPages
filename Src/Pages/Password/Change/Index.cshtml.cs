using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Password.Change;

[Authorize]
public sealed class PasswordChangeViewModel : PageModel
{
}