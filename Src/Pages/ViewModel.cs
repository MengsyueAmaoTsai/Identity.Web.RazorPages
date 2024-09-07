using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RichillCapital.Domain;

namespace RichillCapital.Identity.Web.Pages;

public abstract class ViewModel : PageModel
{
    protected IActionResult Home() => RedirectToPage("/index");
    protected IActionResult Error() => RedirectToPage("/error");
    protected IActionResult SignIn() => RedirectToPage("/signIn/index");
    protected IActionResult SignInPassword(string returnUrl, Email email) => RedirectToPage(
        "/signIn/password/index",
        new
        {
            returnUrl,
            EmailAddress = email.Value,
        });

    protected IActionResult SignInStaySignedIn(string returnUrl, string emailAddress) => RedirectToPage(
        "/signIn/staySignedIn/index",
        new
        {
            returnUrl,
            emailAddress,
        });
}
