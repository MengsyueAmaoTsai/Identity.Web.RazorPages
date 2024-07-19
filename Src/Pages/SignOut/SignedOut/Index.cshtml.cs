using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignOut.SignedOut;

[AllowAnonymous]
public sealed class SignedOutViewModel(
    IIdentityServerInteractionService _interactionService) : 
    ViewModel
{
    public async Task<IActionResult> OnGet(string signOutId)
    {
        var context = await _interactionService.GetLogoutContextAsync(signOutId);

        return Page();
    }
}