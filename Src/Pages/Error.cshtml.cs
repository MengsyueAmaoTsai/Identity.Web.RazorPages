using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
[AllowAnonymous]
public class ErrorViewModel() :
    PageModel
{
    [BindProperty(Name = "errorId", SupportsGet = true)]
    public required string ErrorId { get; init; }

    public required string TraceId { get; set; }

    public IActionResult OnGetAsync()
    {
        TraceId = Activity.Current?.Id ??
            HttpContext.TraceIdentifier;

        return Page();
    }
}

