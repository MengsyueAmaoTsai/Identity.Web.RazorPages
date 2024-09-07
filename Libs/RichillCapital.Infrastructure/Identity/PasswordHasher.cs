using RichillCapital.Domain.Abstractions;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Identity;

internal sealed class PasswordHasher :
    IPasswordHasher
{
    public Result<string> Hash(string password) => Result<string>.With(password);

    public Result VerifyHashedPassword(
        string hashedPassword,
        string providedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
        {
            return Result.Failure(Error.Invalid($"{nameof(hashedPassword)} cannot be null or empty."));
        }

        if (string.IsNullOrEmpty(providedPassword))
        {
            return Result.Failure(Error.Invalid($"{nameof(providedPassword)} cannot be null or empty."));
        }

        return hashedPassword == providedPassword ?
            Result.Success :
            Result.Failure(Error.Invalid("The provided password does not match the hashed password."));
    }
}