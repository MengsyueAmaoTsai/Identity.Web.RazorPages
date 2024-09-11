using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.Password.Reset;

[AllowAnonymous]
public sealed class PasswordResetViewModel(
    ILogger<PasswordResetViewModel> _logger) :
    ViewModel
{
    [BindProperty]
    public required string EmailAddress { get; init; }

    public async Task<IActionResult> OnPostAsync()
    {
        return Page();
    }
}