using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace RichillCapital.Identity.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
[AllowAnonymous]
public sealed class ErrorViewModel(
    ILogger<ErrorViewModel> _logger,
    IIdentityServerInteractionService _interactionService) :
    ViewModel
{
    [BindProperty(Name = "errorId", SupportsGet = true)]
    public required string ErrorId { get; init; }

    public required string RequestId { get; set; }

    public required string ErrorMessage { get; init; }

    public async Task<IActionResult> OnGetAsync()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var message = await _interactionService.GetErrorContextAsync(ErrorId);

        _logger.LogError("ErrorId: {ErrorId}, ErrorMessage: {ErrorMessage}", ErrorId, message?.Error);

        if (message is not null)
        {
        }

        return Page();
    }
}

