using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RichillCapital.Identity.Web.Pages.About;

[Authorize]
public sealed class AboutViewModel(
    ILogger<AboutViewModel> _logger,
    IWebHostEnvironment _environment) :
    ViewModel
{
    public required ApplicationInformationModel ApplicationInformation { get; init; } = new ApplicationInformationModel()
    {
        AssemblyName = Assembly.GetExecutingAssembly()?.GetName().Name ?? string.Empty,
        AssemblyVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty,
        DotNetVersion = RuntimeInformation.FrameworkDescription,
        RuntimeIdentifier = RuntimeInformation.RuntimeIdentifier,
        Environment = _environment.EnvironmentName ?? string.Empty,
    };

    public required string LocalIpAddress { get; set; }
    public required string RemoteIpAddress { get; set; }

    public required IEnumerable<Claim> Claims { get; set; } = [];

    public required IDictionary<string, string?> Properties { get; set; }
    public required string[] ApplicationIds { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        LocalIpAddress = HttpContext.Connection.LocalIpAddress?.ToString() ?? string.Empty;
        RemoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

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
        Properties = result.Properties.Items;

        return Page();
    }
}

public sealed record ApplicationInformationModel
{
    public required string AssemblyName { get; init; }
    public required string AssemblyVersion { get; init; }
    public required string DotNetVersion { get; init; }
    public required string RuntimeIdentifier { get; init; }
    public required string Environment { get; init; }
}