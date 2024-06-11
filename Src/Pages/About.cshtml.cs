using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

[AllowAnonymous]
public class AboutViewModel() :
    PageModel
{
}


internal static class ApplicationInfo
{
    private const string NotPackaged = "Not packaged";

    internal static string GetDisplayName()
    {
        return NotPackaged;
    }

    internal static string GetAssemblyVersion()
    {
        var attribute = typeof(Program).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>() ??
            throw new InvalidOperationException();

        return attribute.InformationalVersion;
    }

    internal static string GetPackageVersion()
    {
        return NotPackaged;
    }

    internal static string GetPackageUri()
    {
        return NotPackaged;
    }

    internal static string GetInstallLocation() => Assembly.GetExecutingAssembly().Location;

    internal static string GetDotNetRuntimeVersion()
    {
        var attribute = typeof(object).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>() ??
            throw new InvalidOperationException();

        return attribute.InformationalVersion;
    }
}
