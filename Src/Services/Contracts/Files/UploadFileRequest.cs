namespace RichillCapital.Identity.Web.Services.Contracts.Files;

public sealed record UploadFileRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IFormFile File { get; init; }
    public required bool Encrypt { get; init; }
}
