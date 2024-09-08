using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages;

[Authorize]
public sealed class HomeViewModel(
    ILogger<HomeViewModel> _logger) :
    ViewModel
{
}