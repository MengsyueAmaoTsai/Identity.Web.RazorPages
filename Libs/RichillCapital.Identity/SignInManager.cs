using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity;

public interface ISignInManager
{
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
}

public interface IUserService
{
}

internal sealed class SignInManager(
    IReadOnlyRepository<User> _userRepository) :
    ISignInManager
{
    public async Task<Result> PasswordSignInAsync(
        Email email,
        string password,
        bool isPersistent,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default)
    {
        //     var userResult = await GetByEmailAsync(email, cancellationToken);

        //     if (userResult.IsFailure)
        //     {
        //         return userResult.Error
        //             .ToResult<UserId>();
        //     }

        //     var user = userResult.Value;

        //     if (password != user.PasswordHash)
        //     {
        //         return Error
        //             .Unauthorized("Users.InvalidCredentials", "Invalid credentials")
        //             .ToResult<UserId>();
        //     }

        //     return user.Id.ToResult();
        // }

        // public Task<Result<UserId>> PasswordSignInAsync(
        //     User user,
        //     bool isPersistent,
        //     bool lockoutOnFailure,
        //     CancellationToken cancellationToken = default)
        // {
        //     throw new NotImplementedException();
        // }

        // private async Task<Result<User>> GetByEmailAsync(
        //     Email email,
        //     CancellationToken cancellationToken = default)
        // {
        //     var maybeUser = await _userRepository
        //         .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        //     if (maybeUser.IsNull)
        //     {
        //         return Error
        //             .NotFound("Users.NotFound", $"User wiht email {email} not found")
        //             .ToResult<User>();
        //     }

        //     var user = maybeUser.Value;

        //     return user.ToResult();
        return Result.Success;
    }

    public Task<Result> PasswordSignInAsync(
        User user,
        string password,
        bool isPersistent,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
