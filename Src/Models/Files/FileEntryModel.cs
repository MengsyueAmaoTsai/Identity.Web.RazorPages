namespace RichillCapital.Identity.Web.Models.Files;

public sealed record FileEntryModel
{
    public required Guid Id { get; init; }
    public required string FileName { get; init; }
    public required string Description { get; init; }
    public required long Size { get; init; }
    public required DateTimeOffset UploadedTime { get; init; }
}
