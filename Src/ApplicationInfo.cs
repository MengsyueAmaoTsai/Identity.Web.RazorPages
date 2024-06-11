using System.Reflection;

namespace RichillCapital.Identity.Web;

internal static class ApplicationInfo
{
    internal static string GetDisplayName()
    {
        return typeof(Program).Assembly.GetName().Name ?? throw new InvalidOperationException();
    }

    internal static string GetAssemblyVersion()
    {
        var attribute = typeof(Program).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>() ??
            throw new InvalidOperationException();

        return attribute.InformationalVersion;
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
