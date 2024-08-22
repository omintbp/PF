using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Volunteers;

public record Experience
{
    private Experience(int value)
    {
        Value = value;
    }
    
    public int Value { get; }

    private static Result<Experience, Error> Create(int value)
    {
        if (value < 0)
            return Errors.General.ValueIsInvalid(nameof(Experience));

        var experience = new Experience(value);
        
        return experience;
    }
}