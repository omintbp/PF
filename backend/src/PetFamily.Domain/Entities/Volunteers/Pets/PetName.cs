using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Volunteers.Pets;

public record PetName
{
    private PetName(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<PetName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(PetName));

        var name = new PetName(value);

        return name;
    }
}