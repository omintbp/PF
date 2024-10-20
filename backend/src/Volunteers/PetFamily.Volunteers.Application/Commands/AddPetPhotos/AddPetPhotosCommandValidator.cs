using FluentValidation;
using Microsoft.Extensions.Options;
using PetFamily.Core.Options;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.AddPetPhotos;

public class AddPetPhotosCommandValidator : AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator(IOptions<ImageUploadOptions> options)
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleForEach(c => c.Photos).SetValidator(new FileValidator(options));
    }
}