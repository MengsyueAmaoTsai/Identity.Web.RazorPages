using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

public abstract class IdentityViewModel : ViewModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }
}


public abstract class ViewModel : PageModel
{
    protected IActionResult RedirectToErrorPage() => RedirectToPage("/error/index");
    protected IActionResult RedirectToProfilePage() => RedirectToPage("/profile/index");
}