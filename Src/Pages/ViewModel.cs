using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

public abstract class ViewModel : PageModel
{
    protected IActionResult Home() => RedirectToPage("/index");
    protected IActionResult Error() => RedirectToPage("/error");
}
