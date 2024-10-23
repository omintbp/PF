using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public record FullName
{
    private FullName(string firstName, string surname, string? patronymic)
    {
        FirstName = firstName;
        Surname = surname;
        Patronymic = patronymic;
    }

    public string FirstName { get; }

    public string Surname { get; }

    public string? Patronymic { get; }

    public static Result<FullName, Error> Create(string firstName, string surname, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(firstName));

        if (string.IsNullOrWhiteSpace(surname) || surname.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(surname));

        if (patronymic?.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(patronymic));

        var fullName = new FullName(firstName, surname, patronymic);

        return fullName;
    }
}