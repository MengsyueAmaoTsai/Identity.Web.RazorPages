using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class ResendEmailConfirmationViewModel : PageModel
{
    [BindProperty]
    public required string Email { get; init; }
}