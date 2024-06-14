using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Services;
using RichillCapital.Identity.Web.Services.Contracts.Files;

namespace RichillCapital.Identity.Web.Pages.Files;

public sealed class UploadFileViewModel(
    ILogger<UploadFileViewModel> _logger,
    IApiService _apiService) : 
    PageModel
{
    [BindProperty]
    public required string Name { get; init; }

    [BindProperty]
    public required string Description { get; init; }

    [Display(Name = "Choose a file")]
    [BindProperty]
    public required IFormFile FromFile { get; init; }

    [BindProperty]
    public required bool Encrypt { get; init; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var uploadResult = await _apiService.UploadFileAsync(
            new UploadFileRequest
            {
                Name = Name,
                Description = Description,
                File = FromFile,
                Encrypt = Encrypt,
            }, 
            cancellationToken);

        if (uploadResult.IsFailure)
        {
            _logger.LogWarning("Failed to upload file {error}", uploadResult.Error);
            return Page();
        }

        var fileId = uploadResult.Value;

        _logger.LogInformation("File with id {FileId} uploaded", fileId);

        return RedirectToPage("./index");
    }
}
