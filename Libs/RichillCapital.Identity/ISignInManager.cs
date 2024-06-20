using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity;

public interface ISignInManager
{
    Task<Result> SignInAsync(
        User user,
        bool isPersistent,
        string? authenticationMethod = default,
        CancellationToken cancellationToken = default);

    Task<Result> PasswordSignInAsync(
        Email email,
        string password,
        bool isPersistent,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default);

    Task<Result> PasswordSignInAsync(
        User user,
        string password,
        bool isPersistent,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default);

    Task<Result> SignOutAsync(CancellationToken cancellationToken = default);
}
