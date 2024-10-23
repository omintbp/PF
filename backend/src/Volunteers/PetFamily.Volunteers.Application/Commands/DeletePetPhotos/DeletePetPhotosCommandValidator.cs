using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.DeletePetPhotos;

public class DeletePetPhotosCommandValidator : AbstractValidator<DeletePetPhotosCommand>
{
    public DeletePetPhotosCommandValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(v => v.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleForEach(v => v.PhotosIds).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}