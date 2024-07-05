using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Error;

[AllowAnonymous]
[IgnoreAntiforgeryToken]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorViewModel() :
    PageModel
{
    public required string ErrorCode { get; set; }
    public required string RequestId { get; set; }
    public required string CorrelationId { get; set; }
    public required DateTimeOffset Timestamp { get; set; }

    [BindProperty(Name = "errorId", SupportsGet = true)]
    public required string ErrorId { get; init; }
    public required string ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        var context = HttpContext;

        if (context is null)
        {
            return Page();
        }

        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            CorrelationId = Guid.NewGuid().ToString();
        }

        CorrelationId = correlationId!;
        RequestId = Activity.Current?.Id ?? context.TraceIdentifier;
        ErrorCode = context.Response.StatusCode.ToString();
        Timestamp = DateTimeOffset.UtcNow;

        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        ErrorMessage = exceptionFeature?.Error.Message ??
            "An error occurred while processing your request.";

        return Page();
    }
}

