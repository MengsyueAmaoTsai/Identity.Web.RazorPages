using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(user => user.Id);

        builder
            .HasIndex(user => user.Email);

        builder
            .Property(user => user.Id)
            .HasMaxLength(UserId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(user => user.Email)
            .HasMaxLength(Email.MaxLength)
            .HasConversion(
                email => email.Value,
                value => Email.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder.HasData([
            CreateUser("1", "Richill Capital", "mengsyue.tsai@outlook.com", "among7201"),
            CreateUser("2", "Mengsyue Amao Tsai", "mengsyue.tsai@gmail.com", "among7201"),
        ]);
    }

    private static User CreateUser(string id, string name, string email, string passwordHash) =>
        User
            .Create(
                UserId.From(id).ThrowIfFailure().Value,
                name,
                Email.From(email).ThrowIfFailure().Value,
                passwordHash)
            .ThrowIfError().Value;
}