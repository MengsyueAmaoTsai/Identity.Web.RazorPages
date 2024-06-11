using FluentValidation;

namespace RichillCapital.Identity.Web.Services;

internal sealed record ResourceServerOptions
{
    internal const string SectionKey = "ResourceServer";

    public required string BaseAddress { get; init; }
}

internal sealed class ResourceServerOptionsValidator :
    AbstractValidator<ResourceServerOptions>
{
    public ResourceServerOptionsValidator()
    {
    }
}