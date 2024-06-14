using RichillCapital.Identity.Web.Services.Contracts;

namespace RichillCapital.Identity.Web.Models.Users;

public sealed record UserModel
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string AvatarUrl { get; set; } 
}

internal static class UserModelMapping
{
    internal static UserModel ToModel(this UserResponse user) =>
        new()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            AvatarUrl = "https://gravatar.com/avatar/0dbe5a836a3b9648db01987c9384c35c?s=200&d=robohash&r=x",
        };
}