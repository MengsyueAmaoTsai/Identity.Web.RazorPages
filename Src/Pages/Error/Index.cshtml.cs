using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Error;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
[AllowAnonymous]
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
        if (HttpContext is not null)
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            ErrorMessage = exceptionFeature?.Error.Message ?? 
                "An error occurred while processing your request.";
            
            ErrorCode = HttpContext.Response.StatusCode.ToString() ?? string.Empty;
            RequestId = HttpContext.Items["RequestId"] as string ?? string.Empty;
            CorrelationId = HttpContext.Items["CorrelationId"] as string ?? string.Empty;
        }

        Timestamp = DateTimeOffset.UtcNow;
        return Page();
    }
}

