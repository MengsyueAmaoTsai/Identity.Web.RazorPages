using Microsoft.AspNetCore.Authorization;

namespace RichillCapital.Identity.Web.Pages.Profile;

[Authorize]
public sealed class ProfileViewModel(
    ILogger<ProfileViewModel> _logger) :
    ViewModel
{
}