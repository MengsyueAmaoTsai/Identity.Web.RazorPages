using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users;

[Authorize]
public sealed class UserProfileViewModel() :
    PageModel
{
}