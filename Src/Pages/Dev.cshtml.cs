using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

public sealed class DevViewModel : PageModel
{
    private static class ErrorMessages
    {
        internal const string InvalidEmail = 
            "Enter a valid email address, phone number, or Skype name.";

        internal const string EmailNotFound = 
            "We couldn't find an account with that username. Try another, or <a>get a new Microsoft account</a>.";
    }
        
    public IActionResult OnPostBack()
    {
        Console.WriteLine("OnPostBack");
        return Page();
    }

    public IActionResult OnPostNext()
    {
        Console.WriteLine("OnPostEmail");
        return Page();
    }
}