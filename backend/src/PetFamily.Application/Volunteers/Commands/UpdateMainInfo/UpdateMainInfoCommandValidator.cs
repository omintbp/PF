using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(r => r.Description).MustBeValueObject(Description.Create);

        RuleFor(r => r.Experience).MustBeValueObject(Experience.Create);

        RuleFor(r => r.FullName)
            .MustBeValueObject(n => FullName.Create(n.FirstName, n.Surname, n.Patronymic));
    }
}