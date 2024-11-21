using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Domain.ValueObjects;

public class Text : ValueObject
{
    private Text(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Text, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(Text));

        return new Text(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}