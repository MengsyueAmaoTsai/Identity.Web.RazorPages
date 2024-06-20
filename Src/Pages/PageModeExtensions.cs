using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages;

internal static class PageModeExtensions
{
    internal static RedirectToPageResult LoadingPage(
        this PageModel page,
        string redirectUri)
    {
        // page.HttpContext.Response.StatusCode = 200;
        // page.HttpContext.Response.Headers["Location"] = "";

        return page.RedirectToPage("/redirect/index", new { RedirectUri = redirectUri });
    }
}
