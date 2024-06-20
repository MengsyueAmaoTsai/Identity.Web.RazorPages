using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity;

internal sealed class UserManager(
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) :
    IUserManager
{
    public async Task<Result> ChangePasswordAsync(
        User user,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        return Result.Success;
    }

    public Task<Result> ConfirmEmailAsync(
        User user,
        string token,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<User>> GetByEmailAsync(
        Domain.Users.Email email,
        CancellationToken cancellationToken = default) =>
        await _userRepository
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken)
            .ToResult(Error.NotFound("Users.NotFound", $"User with email {email} was not found."));

    public async Task<Result<User>> GetByIdAsync(
        UserId id,
        CancellationToken cancellationToken = default) =>
        await _userRepository
            .GetByIdAsync(id, cancellationToken)
            .ToResult(Error.NotFound("Users.NotFound", $"User with id {id} was not found."));
}