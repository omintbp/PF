using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public class Requisite : ValueObject
{
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }

    public string Description { get; }

    public static Result<Requisite, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(name));

        if (string.IsNullOrWhiteSpace(description) || name.Length > Constants.MAX_MEDIUM_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(description));

        var requisite = new Requisite(name, description);

        return requisite;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Name;

        yield return Description;
    }
}