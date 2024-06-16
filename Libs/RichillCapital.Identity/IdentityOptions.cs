using FluentValidation;

namespace RichillCapital.Identity;

public sealed record IdentityOptions
{
    internal const string SectionKey = "Identity";

    public required string IssuerUri { get; init; }
    
    public required ExternalAuthenticationOptions External { get; init; }
}

public sealed record ExternalAuthenticationOptions
{
    public required ExternalOpenIdConnectOptions Microsoft { get; init; }
    public required ExternalOpenIdConnectOptions Google { get; init; }
}

public sealed record ExternalOpenIdConnectOptions
{
    public required string ClientId { get; init; }
    
    public required string ClientSecret { get; init; }

    public required bool RequireHttpsMetadata { get; init; }
}

internal sealed class IdentityOptionsValidator :
    AbstractValidator<IdentityOptions>
{
    public IdentityOptionsValidator()
    {
    }
}

