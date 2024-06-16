using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity.Manage;

[Authorize]
public sealed class ChangePasswordViewModel : PageModel
{

}