using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain.Abstractions;

public interface IPasswordHasher
{
    Result<string> HasPassword(User user, string password);
    Result VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
}