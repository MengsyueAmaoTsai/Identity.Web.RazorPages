namespace RichillCapital.Infrastructure.Identity.Server;

public sealed record IdentityServerOptions
{
    internal const string SectionKey = "Identity:Server";

    public required string IssuerUri { get; init; }
}