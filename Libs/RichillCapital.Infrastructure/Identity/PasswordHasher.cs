using System.Security.Cryptography;
using RichillCapital.Domain.Abstractions;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Identity;

internal sealed class PasswordHasher :
    IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10_000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public Result<string> Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        var hashString = $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";

        return Result<string>.With(hashString);
    }

    public Result VerifyHashedPassword(
        string? hashedPassword,
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

        string[] parts = hashedPassword.Split('-');

        if (parts.Length != 2)
        {
            return Result.Failure(Error.Invalid("Invalid hashed password format."));
        }

        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] testHash = Rfc2898DeriveBytes.Pbkdf2(providedPassword, salt, Iterations, Algorithm, HashSize);

        return Result.Success;
        return hash.SequenceEqual(testHash) ?
            Result.Success :
            Result.Failure(Error.Invalid("Password is invalid."));
    }
}