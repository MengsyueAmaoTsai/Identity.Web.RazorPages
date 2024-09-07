using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain.Abstractions;

public interface IPasswordHasher
{
    Result<string> Hash(string password);
    Result VerifyHashedPassword(string hashedPassword, string providedPassword);
}