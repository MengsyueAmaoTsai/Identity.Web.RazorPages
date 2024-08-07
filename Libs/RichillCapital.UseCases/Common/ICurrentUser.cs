using RichillCapital.Domain.Users;

namespace RichillCapital.UseCases.Common;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    UserId Id { get; }
}