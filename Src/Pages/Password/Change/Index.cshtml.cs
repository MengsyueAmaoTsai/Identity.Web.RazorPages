using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.Password.Change;

public sealed class PasswordChangeViewModel(
    ILogger<PasswordChangeViewModel> _logger) :
    ViewModel
{
    [BindProperty]
    [Required(ErrorMessage = "This information is required.")]
    public required string CurrentPassword { get; init; }

    [BindProperty]
    [Required(ErrorMessage = "This information is required.")]
    public required string NewPassword { get; init; }

    [BindProperty]
    public required string ReenterPassword { get; init; }

    public IActionResult OnPost()
    {
        // Cancel to /security/index
        _logger.LogInformation("Password change request received. not implemented yet.");
        return Page();
    }
}