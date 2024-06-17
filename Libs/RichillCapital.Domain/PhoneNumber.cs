using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class PhoneNumber : SingleValueObject<string>
{
    public const int MaxLength = 100;

    public static readonly PhoneNumber Empty = new(string.Empty);

    private PhoneNumber(string value)
        : base(value)
    {
    }

    public static Result<PhoneNumber> From(string value) => value
        .ToResult()
        .Then(number => new PhoneNumber(number));
}