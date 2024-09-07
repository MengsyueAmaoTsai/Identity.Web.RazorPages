using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace RichillCapital.Identity.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public sealed class ErrorViewModel() : ViewModel
{
    public required string RequestId { get; set; }

    public IActionResult OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        return Page();
    }
}

