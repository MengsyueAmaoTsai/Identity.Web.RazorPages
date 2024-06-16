using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class ForgoetPasswordViewModel
{
    [BindProperty]
    public required string Email { get; init; }
}