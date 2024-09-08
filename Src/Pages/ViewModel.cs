using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RichillCapital.Domain;

namespace RichillCapital.Identity.Web.Pages;

public abstract class ViewModel : PageModel
{
    protected IActionResult Home() => RedirectToPage("/index");
    protected IActionResult Error() => RedirectToPage("/error/index");

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

    protected IActionResult SignUpCreatePassword(string returnUrl, Email email) => RedirectToPage(
        "/signUp/createPassword/index",
        new
        {
            returnUrl,
            EmailAddress = email.Value,
        });

    protected IActionResult SignUpBirthdate(string returnUrl, string emailAddress, string name) => RedirectToPage(
        "/signUp/birthdate/index",
        new
        {
            returnUrl,
            emailAddress,
            name,
        });

    protected IActionResult SignUpVerifyEmail() =>
        RedirectToPage("/signUp/verifyEmail/index");
}
