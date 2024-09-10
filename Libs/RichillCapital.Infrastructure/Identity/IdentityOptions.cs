using FluentValidation;
using RichillCapital.Infrastructure.Identity.Server;

namespace RichillCapital.Infrastructure.Identity;

public sealed record IdentityOptions
{
    internal const string SectionKey = "Identity";

    public required IdentityServerOptions Server { get; init; }
}

internal sealed class IdentityOptionsValidator : AbstractValidator<IdentityOptions>
{
    public IdentityOptionsValidator()
    {
        RuleFor(options => options.Server)
            .NotNull()
            .WithMessage("Identity server options must be provided.");
    }
}