using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users.Challenge;

[AllowAnonymous]
public sealed class ChallengeViewModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public required string Provider { get; set; }

    public IActionResult OnGet()
    {
        return Challenge(Provider);
    }
}