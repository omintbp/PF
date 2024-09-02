using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Delete;

public class DeleteFileRequestValidator : AbstractValidator<DeleteFileRequest>
{
    public DeleteFileRequestValidator()
    {
        RuleFor(d => d.ObjectName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(d => d.BucketName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}