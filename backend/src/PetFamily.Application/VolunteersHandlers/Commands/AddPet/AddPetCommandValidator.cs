using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.VolunteersHandlers.Commands.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
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
                    d.Birthday));
    }
}