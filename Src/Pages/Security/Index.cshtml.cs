using Microsoft.AspNetCore.Authorization;

namespace RichillCapital.Identity.Web.Pages.Security;

[Authorize]
public sealed class SecurityViewModel :
    ViewModel
{
}