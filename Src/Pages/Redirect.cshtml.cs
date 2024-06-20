using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

public sealed class RedirectViewModel() :
    PageModel
{
    public required string RedirectUri { get; init; }
}