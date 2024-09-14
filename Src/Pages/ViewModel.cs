using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RichillCapital.Domain;

namespace RichillCapital.Identity.Web.Pages;

public abstract class ViewModel : PageModel
{
    protected IActionResult Home() => RedirectToPage("/index");
    protected IActionResult Error() => RedirectToPage("/errors/index");

    protected IActionResult Redirecting(string redirectUri)
    {
        HttpContext.Response.StatusCode = 200;
        HttpContext.Response.Headers["Location"] = "";

        return RedirectToPage("/redirect/index", new { RedirectUri = redirectUri });
    }

    protected IActionResult SignedOut() => RedirectToPage("/signedOut/index");

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

    protected IActionResult SignUpBirthdate(string returnUrl, string emailAddress) => RedirectToPage(
        "/signUp/birthdate/index",
        new
        {
            returnUrl,
            emailAddress,
        });

    protected IActionResult SignUpVerifyEmail(string returnUrl, string emailAddress) => RedirectToPage(
        "/signUp/verifyEmail/index",
        new
        {
            returnUrl,
            emailAddress,
        });
}
