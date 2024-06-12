using FluentValidation;

namespace RichillCapital.Identity.Web.IdentityServer;

public sealed record IdentityServerOptions
{
    internal const string SectionKey = "IdentityServer";

    public required string IssuerUri { get; init; }

    public required int RememberMeDurationDays { get; init; }
}

internal sealed class IdentityServerOptionsValidator :
    AbstractValidator<IdentityServerOptions>
{
    public IdentityServerOptionsValidator()
    {
    }
}