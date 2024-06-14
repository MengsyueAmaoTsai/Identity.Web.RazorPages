namespace RichillCapital.Identity.Web.Services.Contracts.Users;

public sealed record UserResponse
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
}
