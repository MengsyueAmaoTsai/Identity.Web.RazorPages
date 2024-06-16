using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain.Common.Identity;

public interface ISignInManager
{
    Task<Result<UserId>> PasswordSignInAsync(Email email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default);
    Task<Result<UserId>> PasswordSignInAsync(User user, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default);
}
