using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Domain.ValueObjects;

public record SpeciesName
{
    private SpeciesName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<SpeciesName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(SpeciesName));

        var speciesName = new SpeciesName(value);

        return speciesName;
    }
}