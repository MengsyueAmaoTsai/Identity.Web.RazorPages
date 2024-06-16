using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class SignUpViewModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    public required string Email { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    [BindProperty]
    public required string ConfirmPassword { get; init; }
}