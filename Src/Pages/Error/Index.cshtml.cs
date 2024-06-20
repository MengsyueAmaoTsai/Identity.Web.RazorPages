using System.Diagnostics;

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
    [BindProperty(Name = "errorId", SupportsGet = true)]
    public required string ErrorId { get; init; }

    public required string TraceId { get; set; }

    public required string ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        TraceId = Activity.Current is null ?
            string.Empty :
            HttpContext.TraceIdentifier;

        if (HttpContext is not null)
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            ErrorMessage = exceptionFeature?.Error.Message ?? "An error occurred while processing your request.";
        }

        return Page();
    }
}

