using System.Security.Claims;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Diagnostics;

[Authorize]
public sealed class DiagnosticsViewModel :
    PageModel
{
    public required IEnumerable<string> Clients { get; set; } = [];
    public required IEnumerable<Claim> Claims { get; set; } = [];
    public required IDictionary<string, string?> Properties { get; set; } = new Dictionary<string, string?>();

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await HttpContext.AuthenticateAsync();

        if (result.Properties!.Items.ContainsKey("client_list"))
        {
            var encoded = result.Properties.Items["client_list"] ?? string.Empty;

            var bytes = Convert.FromBase64String(encoded);

            Clients = JsonSerializer.Deserialize<IEnumerable<string>>(Encoding.UTF8.GetString(bytes)) ?? [];
        }

        Claims = result.Principal!.Claims;
        Properties = result.Properties?.Items ?? new Dictionary<string, string?>();

        return Page();
    }
}
