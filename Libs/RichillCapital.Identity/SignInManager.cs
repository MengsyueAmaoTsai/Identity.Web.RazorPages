using System.Security.Claims;

using Duende.IdentityServer;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

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
    IReadOnlyRepository<User> _userRepository,
    IHttpContextAccessor _httpContextAccessor) :
    ISignInManager
{
    public async Task<Result> PasswordSignInAsync(
        Email email,
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
}
