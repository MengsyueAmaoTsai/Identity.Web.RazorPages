using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

public abstract class ViewModel : PageModel
{
    protected IActionResult ErrorPage()
    {
        return RedirectToPage("/errors/index");
    }

    protected IActionResult ProfilePage()
    {
        return RedirectToPage("/profile/index");
    }
}