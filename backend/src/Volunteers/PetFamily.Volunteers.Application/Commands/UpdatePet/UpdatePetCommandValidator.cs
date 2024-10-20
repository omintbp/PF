using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdatePet;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.Name).MustBeValueObject(PetName.Create);

        RuleFor(r => r.Description).MustBeValueObject(Description.Create);

        RuleFor(r => r.Phone).MustBeValueObject(PhoneNumber.Create);

        RuleFor(r => r.Address)
            .MustBeValueObject(a =>
                Address.Create(
                    a.Country,
                    a.City,
                    a.Street,
                    a.House,
                    a.Flat));

        RuleFor(r => r.Details)
            .MustBeValueObject(d =>
                PetDetails.Create(
                    d.Weight,
                    d.Height,
                    d.IsCastrated,
                    d.IsVaccinated,
                    d.Color,
                    d.HealthInfo,
                    d.BirthdayDate));
    }
}