using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users;

[AllowAnonymous]
public sealed class UsersViewModel() :
    PageModel
{
}