using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain.Abstractions;

public interface IUserManager
{
    bool SupportsUserLockout { get; }
    Task<Result> CreateAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<Result<User>> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Result CheckPassword(User user, string password);
    Task<Result> AccessFailedAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> IsLockedOutAsync(User user, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(User user, string currentPassword, string newPassword);
}