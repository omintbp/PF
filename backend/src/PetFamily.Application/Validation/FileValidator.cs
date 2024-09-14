using FluentValidation;
using Microsoft.Extensions.Options;
using PetFamily.Application.Options;
using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Validation;

public class FileValidator : AbstractValidator<FileDto>
{
    public FileValidator(IOptions<ImageUploadOptions> options)
    {
        RuleFor(f => f.FileName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(f => f.Content.Length)
            .NotNull()
            .LessThanOrEqualTo(options.Value.MaxImageSize)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(f => f.FileName).MustBeAllowedExtension(options.Value.AllowedExtensions);
    }
}