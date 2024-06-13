using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Profile;

public sealed class ProfileViewModel : PageModel
{
    public required string PictureUrl { get; set; }
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }

    public IActionResult OnGet()
    {
        Initialize();

        return Page();
    }

    private void Initialize()
    {
        // Fetch user data from current web user (ICurrentUser).

        PictureUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50";
        Id = "1";
        Name = "Tsai Mengsyue";
        Email = "mengsyue.tsai@outlook.com";
        PhoneNumber = "+886903776473";
    }
}