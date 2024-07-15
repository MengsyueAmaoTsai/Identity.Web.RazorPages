using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

public abstract class IdentityViewModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }
}
