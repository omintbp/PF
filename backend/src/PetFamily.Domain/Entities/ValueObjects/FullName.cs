using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.ValueObjects;

public record FullName
{
    private FullName(string firstName, string surname, string patronymic)
    {
        FirstName = firstName;
        Surname = surname;
        Patronymic = patronymic;
    }
    
    public string FirstName { get; }

    public string Surname { get; }

    public string Patronymic { get; }

    public static Result<FullName,Error> Create(string firstName, string surname, string patronymic)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsInvalid(nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(surname))
            return Errors.General.ValueIsInvalid(nameof(surname));
        
        if (string.IsNullOrWhiteSpace(patronymic))
            return Errors.General.ValueIsInvalid(nameof(patronymic));
        
        var fullName = new FullName(firstName, surname, patronymic);
        
        return fullName;
    }
}