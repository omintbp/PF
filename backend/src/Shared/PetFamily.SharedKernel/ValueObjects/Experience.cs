using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public record Experience
{
    public static int MAX_EXPERIENCE = 100;

    private Experience(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<Experience, Error> Create(int value)
    {
        if (value < 0 || value > MAX_EXPERIENCE)
            return Errors.General.ValueIsInvalid(nameof(Experience));

        var experience = new Experience(value);

        return experience;
    }
}