using Microsoft.AspNetCore.Authorization;

namespace RichillCapital.Identity.Web.Pages.SignedOut;

[AllowAnonymous]
public sealed class SignedOutViewModel(
    ILogger<SignedOutViewModel> _logger) :
    ViewModel
{
}