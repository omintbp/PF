using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects;

public class VolunteerInfo : ValueObject
{
    private VolunteerInfo(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<VolunteerInfo, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(VolunteerInfo));

        return new VolunteerInfo(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}