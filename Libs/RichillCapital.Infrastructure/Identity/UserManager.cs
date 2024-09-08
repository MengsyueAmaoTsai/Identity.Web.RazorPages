using Microsoft.Extensions.Logging;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.Domain.Errors;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Identity;

internal sealed class UserManager(
    ILogger<UserManager> _logger,
    IPasswordHasher _passwordHasher,
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) :
    IUserManager
{
    public bool SupportsUserLockout => false;

    public Result CheckPassword(
        User user,
        string password)
    {
        var verifyResult = VerifyPassword(user, password);

        if (verifyResult.IsFailure)
        {
            _logger.LogInformation("Invalid password for user. User {userId}.", user.Id);
            return Result.Failure(verifyResult.Error);
        }

        return Result.Success;
    }

    public async Task<Result> CreateAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default)
    {
        // var passwordStore = GetPasswordStore();

        if (string.IsNullOrEmpty(password))
        {
            return Result.Failure(Error.Invalid($"{nameof(password)} cannot be null or empty."));
        }

        // var result = await UpdatePasswordHash(passwordStore, user, password).ConfigureAwait(false);

        // if (!result.Succeeded)
        // {
        //     return result;
        // }
        return await CreateAsync(user, cancellationToken);
    }

    public async Task<Result> CreateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        // await UpdateSecurityStampInternal(user).ConfigureAwait(false);
        var validateResult = await ValidateUserAsync(user);

        if (validateResult.IsFailure)
        {
            return Result.Failure(validateResult.Error);
        }

        // if (Options.Lockout.AllowedForNewUsers && SupportsUserLockout)
        // {
        //     await GetUserLockoutStore().SetLockoutEnabledAsync(user, true, CancellationToken).ConfigureAwait(false);
        // }

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    public async Task<Result<User>> FindByEmailAsync
    (Email email,
    CancellationToken cancellationToken = default)
    {
        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (maybeUser.IsNull)
        {
            return Result<User>.Failure(UserErrors.NotFound(email));
        }

        var user = maybeUser.Value;

        return Result<User>.With(user);
    }

    private static async Task<Result> ValidateUserAsync(User user)
    {
        await Task.Delay(100);

        return Result.Success;
    }

    private Result VerifyPassword(
        User user,
        string password) =>
        _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

    public Task<Result> AccessFailedAsync(
        User user,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public async Task<bool> IsLockedOutAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        // var store = GetUserLockoutStore();

        // if (!await store.GetLockoutEnabledAsync(user, CancellationToken);
        // {
        //     return false;
        // }

        // var lockoutTime = await store.GetLockoutEndDateAsync(user, CancellationToken).ConfigureAwait(false);

        // return lockoutTime >= DateTimeOffset.UtcNow;
        return false;
    }

    public async Task<Result> ChangePasswordAsync(
        User user,
        string currentPassword,
        string newPassword)
    {
        var verifyResult = VerifyPassword(user, currentPassword);

        if (verifyResult.IsFailure)
        {
            _logger.LogInformation("Change password failed for user.");
            return Result.Failure(verifyResult.Error);
        }

        var updateResult = await UpdatePasswordHash(user, newPassword);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        return await UpdateUserAsync(user);
    }

    private async Task<Result> UpdatePasswordHash(
        User user,
        string? newPassword,
        bool validatePassword = true)
    {
        if (validatePassword)
        {
            var verifyResult = VerifyPassword(user, newPassword);

            if (verifyResult.IsFailure)
            {
                return Result.Failure(verifyResult.Error);
            }
        }

        var hashResult = _passwordHasher.HasPassword(user, newPassword!);

        if (hashResult.IsFailure)
        {
            return Result.Failure(hashResult.Error);
        }

        var hash = hashResult.Value;

        // user.ChangePassword(hash);
        //await UpdateSecurityStampInternal(user).ConfigureAwait(false);
        return Result.Success;
    }

    private async Task<Result> UpdateUserAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success;
    }

    public Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user) =>
        GenerateUserTokenAsync(user, string.Empty, string.Empty);

    public async Task<Result<string>> GenerateUserTokenAsync(
        User user,
        string tokenProvider,
        string purpose)
    {
        if (string.IsNullOrEmpty(tokenProvider))
        {
            return Result<string>.Failure(Error.Invalid($"{nameof(tokenProvider)} cannot be null or empty."));
        }

        // if (!_tokenProviders.TryGetValue(tokenProvider, out var provider))
        // {
        //     throw new NotSupportedException(Resources.FormatNoTokenProvider(nameof(TUser), tokenProvider));
        // }

        return Result<string>.With("0000");
    }

    public async Task<Result> ConfirmEmailAsync(User user, string token)
    {
        // if (!await VerifyUserTokenAsync(user, Options.Tokens.EmailConfirmationTokenProvider, ConfirmEmailTokenPurpose, token).ConfigureAwait(false))
        // {
        //     return IdentityResult.Failed(ErrorDescriber.InvalidToken());
        // }
        // await store.SetEmailConfirmedAsync(user, true, CancellationToken).ConfigureAwait(false);
        return await UpdateUserAsync(user);
    }
}