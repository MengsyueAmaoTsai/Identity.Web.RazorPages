using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

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
    public required string ErrorMessage { get; init; }

    public required string RequestId { get; set; }
    public required string Title { get; set; }
    public required string Detail { get; set; }
    public required HttpStatusCode Status { get; set; }
    public required string Type { get; set; }

    public async Task<IActionResult> OnGetAsync(string errorId)
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var message = await _interactionService.GetErrorContextAsync(errorId);

        if (message is not null)
        {
            _logger.LogInformation("Identity server error. {error} {description}",
                message.Error,
                message.ErrorDescription);
        }

        _logger.LogError("Unexpected error. {title}: {detail}", Title, Detail);

        return Page();
    }
}

