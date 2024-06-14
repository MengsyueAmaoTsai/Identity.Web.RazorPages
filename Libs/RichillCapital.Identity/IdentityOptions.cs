using FluentValidation;

namespace RichillCapital.Identity;

public sealed record IdentityOptions
{
    internal const string SectionKey = "Identity";

    public required string IssuerUri { get; init; }

    public required int RememberMeDurationDays { get; init; }
}

internal sealed class IdentityOptionsValidator :
    AbstractValidator<IdentityOptions>
{
    public IdentityOptionsValidator()
    {
    }
}