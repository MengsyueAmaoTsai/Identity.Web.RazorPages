using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity.Profile;

[Authorize]
public sealed class ChangePasswordViewModel : PageModel
{

}