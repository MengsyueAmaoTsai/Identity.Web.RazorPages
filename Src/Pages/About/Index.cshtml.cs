using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.About;

[AllowAnonymous]
public class AboutViewModel() :
    PageModel
{
}
