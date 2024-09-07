using Microsoft.AspNetCore.Authorization;

namespace RichillCapital.Identity.Web.Pages.SignUp;

[AllowAnonymous]
public sealed class SignUpViewModel(
    ILogger<SignUpViewModel> _logger) :
    ViewModel
{
}