using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.VolunteersHandlers.Commands.DeletePetPhotos;

public class DeletePetPhotosCommandValidator : AbstractValidator<DeletePetPhotosCommand>
{
    public DeletePetPhotosCommandValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleForEach(v => v.PhotosIds).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}