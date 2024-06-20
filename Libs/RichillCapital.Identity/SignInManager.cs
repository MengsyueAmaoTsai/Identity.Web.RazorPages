using Duende.IdentityServer;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

using static Duende.IdentityServer.Models.IdentityResources;

namespace RichillCapital.Identity;

public interface ISignInManager
{
    Task<Result> PasswordSignInAsync(
        Domain.Users.Email email,
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
}

internal sealed class SignInManager(
    IReadOnlyRepository<User> _userRepository,
    IHttpContextAccessor _httpContextAccessor) :
    ISignInManager
{
    public async Task<Result> PasswordSignInAsync(
        Domain.Users.Email email,
        string password,
        bool isPersistent,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default)
    {
        // Find user by email
        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken);

        var user = maybeUser.ValueOrDefault;

        // Check password
        if (password != user.PasswordHash)
        {
            return Error
                .Unauthorized("Users.InvalidCredentials", "Invalid credentials")
                .ToResult();
        }

        // Sign in user 
        var properties = isPersistent ?
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
            } :
            new AuthenticationProperties();

        var identityServerUser = new IdentityServerUser(user.Id.Value)
        {
            DisplayName = user.Name.Value,
        };

        await _httpContextAccessor.HttpContext.SignInAsync(identityServerUser, properties);

        // var claims = new List<Claim>
        // {
        //    new("sub", user.Id.Value),
        //    new("name", user.Name.Value),
        //    new("email", user.Email.Value),
        // };

        // var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "idsrv"));
        // await _httpContextAccessor.HttpContext.SignInAsync(principal, properties);
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

    public async Task<Result> SignOutAsync(CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return Result.Failure(Error.Unexpected("HttpContext is null"));
        }

        await _httpContextAccessor.HttpContext.SignOutAsync();

        return Result.Success;
    }
}

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