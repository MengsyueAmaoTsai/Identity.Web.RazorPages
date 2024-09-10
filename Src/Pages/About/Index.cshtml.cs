using System.Security.Claims;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RichillCapital.Identity.Web.Pages.About;

public sealed class AboutViewModel(
    ILogger<AboutViewModel> _logger) :
    ViewModel
{
    public required string[] ApplicationIds { get; set; }
    public required IEnumerable<Claim> Claims { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var localAddress = new string[]
        {
            "127.0.0.1",
            "::1",
            HttpContext.Connection.LocalIpAddress?.ToString() ?? string.Empty,
        };

        if (!localAddress.Contains(HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            return NotFound();
        }

        var result = await HttpContext.AuthenticateAsync();

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to authenticate user.");

            return Page();
        }

        if (result.Properties.Items.ContainsKey("client_list"))
        {
            var encoded = result.Properties.Items["client_list"] ?? string.Empty;
            var bytes = Base64Url.Decode(encoded);
            var value = Encoding.UTF8.GetString(bytes);

            ApplicationIds = JsonConvert.DeserializeObject<string[]>(value) ?? [];
        }

        Claims = result.Principal.Claims;

        return Page();
    }
}