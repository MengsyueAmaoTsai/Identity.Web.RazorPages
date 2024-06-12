using System.Diagnostics;

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
[AllowAnonymous]
public class ErrorViewModel(
    IIdentityServerInteractionService _interactionService) :
    PageModel
{
    [BindProperty(Name = "errorId", SupportsGet = true)]
    public required string ErrorId { get; init; }

    public required ErrorMessage IdentityErrorMessage { get; set; }

    public required string TraceId { get; set; }

    public required string ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var message = await _interactionService.GetErrorContextAsync(ErrorId);

        if (message is not null)
        {
            IdentityErrorMessage = message;
        }

        TraceId = Activity.Current?.Id ??
            HttpContext.TraceIdentifier;

        return Page();
    }
}

