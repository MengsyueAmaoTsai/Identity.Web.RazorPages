using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Identity.Web.Models.Files;
using RichillCapital.Identity.Web.Services;

namespace RichillCapital.Identity.Web.Pages.Files;

public sealed class ListFilesViewModel(
    IApiService _apiService) : 
    PageModel
{
    public required IEnumerable<FileEntryModel> Files { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var filesResult = await _apiService.ListFileEntriesAsync(cancellationToken);

        if (filesResult.IsFailure)
        {
            return RedirectToPage("/Error");
        }

        var files = filesResult.Value;

        Files = files
            .Select(file => new FileEntryModel
            { 
                Id = file.Id,
                FileName = file.FileName,
                Description = file.Description,
                Size = file.Size,
                UploadedTime = file.UploadedTime,
            });

        return Page();
    }
}

