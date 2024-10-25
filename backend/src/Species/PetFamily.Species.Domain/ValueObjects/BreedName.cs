using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Domain.ValueObjects;

public record BreedName
{
    private BreedName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<BreedName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(BreedName));

        var breedName = new BreedName(value);

        return breedName;
    }
}