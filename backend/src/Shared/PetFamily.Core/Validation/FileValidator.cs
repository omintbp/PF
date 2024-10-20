using FluentValidation;
using Microsoft.Extensions.Options;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Validation;

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