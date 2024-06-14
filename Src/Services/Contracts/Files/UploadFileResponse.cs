namespace RichillCapital.Identity.Web.Services.Contracts.Files;

public sealed record UploadFileResponse
{
    public required Guid Id { get; init; }
}
