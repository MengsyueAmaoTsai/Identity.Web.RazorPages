using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichillCapital.Domain.Abstractions;

namespace RichillCapital.Identity.Web.Pages.SignOut;

[Authorize]
public sealed class SignOutViewModel(
    ILogger<SignOutViewModel> _logger,
    ISignInManager _signInManager) : ViewModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        await _signInManager.SignOutAsync();

        return Home();
    }
}