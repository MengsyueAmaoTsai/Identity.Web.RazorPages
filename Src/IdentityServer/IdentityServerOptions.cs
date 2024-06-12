using FluentValidation;

namespace RichillCapital.Identity.Web.IdentityServer;

internal sealed record IdentityServerOptions
{
    internal const string SectionKey = "IdentityServer";

    internal required string IssuerUri { get; init; }
}

internal sealed class IdentityServerOptionsValidator :
    AbstractValidator<IdentityServerOptions>
{
    internal IdentityServerOptionsValidator()
    {
        RuleFor(options => options.IssuerUri)
            .NotEmpty()
            .WithMessage("Issuer URI is required.");
    }
}
