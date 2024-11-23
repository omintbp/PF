using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects;

public class RejectionComment : ValueObject
{
    public static RejectionComment None => new (string.Empty);
    
    private RejectionComment(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<RejectionComment, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(VolunteerInfo));

        return new RejectionComment(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}