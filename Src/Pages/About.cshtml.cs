using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

[AllowAnonymous]
public class AboutViewModel(
    ILogger<AboutViewModel> _logger) :
    PageModel
{
    public void OnGet()
    {
    }
}

