using Microsoft.AspNetCore.Authentication;
using RichillCapital.SharedKernel.Monads;
using System.Security.Claims;

namespace RichillCapital.Domain.Abstractions;

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

    Task<Result> CheckPasswordSignInAsync(
        User user,
        string password,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default);

    Task<Result> SignInOrTwoFactorAsync(
        User user,
        bool isPersistent,
        string? loginProvider = null,
        bool bypassTwoFactor = false);

    Task SignInWithClaimsAsync(
        User user,
        AuthenticationProperties? authenticationProperties,
        IEnumerable<Claim> additionalClaims);

    Task SignInWithClaimsAsync(
        User user,
        bool isPersistent,
        IEnumerable<Claim> additionalClaims);

    Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user);

    Task SignOutAsync();
}