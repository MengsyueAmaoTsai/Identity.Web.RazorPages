namespace RichillCapital.Identity.Web.Services.Contracts.Files;

public record FileEntryResponse
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required long Size { get; set; }

    public required DateTimeOffset UploadedTime { get; set; }

    public required string FileName { get; set; }

    public required string FileLocation { get; set; }

    public required bool Encrypted { get; set; }
}
