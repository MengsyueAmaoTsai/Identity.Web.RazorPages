using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.SharedKernel;

namespace RichillCapital.Identity.Web.Pages.Errors;

public sealed class ErrorsViewModel() : PageModel
{
    public required string RequestId { get; init; }
    public required string CorrelationId { get; init; }
    public required Error Error { get; init; }

    public IActionResult OnGet()
    {
        return Page();
    }
}