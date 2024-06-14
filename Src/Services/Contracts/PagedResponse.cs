namespace RichillCapital.Identity.Web.Services.Contracts;

public sealed record PagedResponse<T>
{
    public required IEnumerable<T> Items { get; init; }
}