using RichillCapital.SharedKernel;

namespace RichillCapital.Domain;

public sealed class User : Entity<UserId>
{
    public User(
        UserId id,
        string name,
        Email email,
        string passwordHash,
        DateTimeOffset createdTimeUtc) 
        : base(id)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        CreateTimeUtc = createdTimeUtc;
    }

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTimeOffset CreateTimeUtc { get; private set; }
}
