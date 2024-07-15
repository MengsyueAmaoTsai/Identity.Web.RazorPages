using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp;

public sealed class SignUpSuccessViewModel : ViewModel
{
    public IActionResult OnPost()
    {
        return RedirectToProfilePage();
    }
}