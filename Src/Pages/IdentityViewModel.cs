using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages;

public abstract class IdentityViewModel : ViewModel
{
    [BindProperty(Name = "returnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; set; }
}