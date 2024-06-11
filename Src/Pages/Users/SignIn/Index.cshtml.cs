using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users.SignIn;

public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger) :
    PageModel
{
    [BindProperty(Name = "ReturnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "Email")]
    public required string Email { get; init; }

    [BindProperty(Name = "Password")]
    public required string Password { get; init; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        return Page();
    }
}