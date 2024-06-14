using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

[Authorize]
public class IndexViewModel : PageModel
{
    public void OnGet()
    {
    }
}
