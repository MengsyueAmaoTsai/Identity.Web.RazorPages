using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignUp.BirthDate;

public sealed class BirthDateViewModel : IdentityViewModel
{
    [BindProperty]
    public required int BirthYear { get; init; } = DateTimeOffset.UtcNow.Year;
}