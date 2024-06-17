using Microsoft.Extensions.DependencyInjection;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public interface IUserService
{
    Task<Result<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(User user, string oldPassword, string newPassword, CancellationToken cancellationToken = default);
}

internal sealed class UserService(
    IRepository<User> _repository,
    IUnitOfWork _unitOfWork) :
    IUserService
{
    public async Task<Result> ChangePasswordAsync(
        User user,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        return Result.Success;
    }

    public async Task<Result<User>> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default) =>
        await _repository
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken)
            .ToResult(Error.NotFound(
                "Users.NotFound",
                $"User with email {email} was not found."));

    public async Task<Result<User>> GetByIdAsync(
        UserId id,
        CancellationToken cancellationToken = default) =>
        await _repository
            .GetByIdAsync(id, cancellationToken)
            .ToResult(Error.NotFound(
                "Users.NotFound",
                $"User with id {id} was not found."));
}

public static class UserServiceExtensions
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}