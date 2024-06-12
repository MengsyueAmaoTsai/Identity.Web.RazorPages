namespace RichillCapital.Identity.Web.Pages.Users.Consent;

public sealed record IdentityResourceModel
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required string Value { get; init; }
    public required bool Required { get; init; }
    public required bool Emphasize { get; init; }
    public required bool IsConsented { get; init; }
}