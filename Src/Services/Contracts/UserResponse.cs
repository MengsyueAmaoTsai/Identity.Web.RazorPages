namespace RichillCapital.Identity.Web.Services.Contracts;

public sealed record UserResponse
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
}

public sealed record PagedResponse<T>
{
    public required IEnumerable<T> Items { get; init; }
}