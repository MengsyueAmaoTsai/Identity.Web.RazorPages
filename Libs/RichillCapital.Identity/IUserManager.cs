using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity;

public interface IUserManager
{
    Task<Result> ChangePasswordAsync(
        User user,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    Task<Result> ConfirmEmailAsync(User user, string token, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<string> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken = default);
}
