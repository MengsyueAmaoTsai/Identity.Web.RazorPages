using Microsoft.AspNetCore.Authorization;

namespace RichillCapital.Identity.Web.Pages;

[Authorize]
public sealed class HomeViewModel(
    ILogger<HomeViewModel> _logger) :
    ViewModel
{
}