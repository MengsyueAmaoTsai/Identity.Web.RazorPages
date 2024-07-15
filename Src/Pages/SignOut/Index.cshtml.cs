using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.SignOut;

public sealed class SignOutViewModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string PostSignOutRedirectUri { get; init; }
}