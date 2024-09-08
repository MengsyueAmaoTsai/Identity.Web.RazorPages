using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RichillCapital.Domain;

namespace RichillCapital.Identity.Web.Pages;

public abstract class ViewModel : PageModel
{
    protected IActionResult Home() => RedirectToPage("/index");
    protected IActionResult Error() => RedirectToPage("/error/index");

    protected IActionResult SignIn(string returnUrl) => RedirectToPage(
        "/signIn/index",
        new
        {
            returnUrl,
        });

    protected IActionResult SignUp(string returnUrl) => RedirectToPage(
        "/signUp/index",
        new
        {
            returnUrl,
        });

    protected IActionResult Profile() => RedirectToPage("/profile/index");

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

    protected IActionResult SignUpName(string returnUrl, string emailAddress) => RedirectToPage(
        "/signUp/name/index",
        new
        {
            returnUrl,
            emailAddress,
        });

    protected IActionResult SignUpBirthdate(string returnUrl, string emailAddress, string name) => RedirectToPage(
        "/signUp/birthdate/index",
        new
        {
            returnUrl,
            emailAddress,
            name,
        });

    protected IActionResult PasswordChange() => RedirectToPage("/password/change/index");
}
