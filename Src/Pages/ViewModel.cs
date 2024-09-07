using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

public abstract class ViewModel : PageModel
{
    protected IActionResult Home() => RedirectToPage("/index");
    protected IActionResult Error() => RedirectToPage("/error");
    protected IActionResult SignIn() => RedirectToPage("/signIn/index");
    protected IActionResult SignInPassword() => RedirectToPage("/signIn/password/index");
    protected IActionResult SignInStaySignedIn() => RedirectToPage("/signIn/staySignedIn/index");
}
