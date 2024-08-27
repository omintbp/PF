using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Requisite
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
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid(nameof(description));

        var requisite = new Requisite(name, description);

        return requisite;
    }
}