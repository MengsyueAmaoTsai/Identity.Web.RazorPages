using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.About;

[Authorize]
public class AboutViewModel() :
    PageModel
{
}
