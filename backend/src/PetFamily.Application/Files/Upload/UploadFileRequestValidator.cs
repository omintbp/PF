using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Upload;

public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequestValidator()
    {
        RuleFor(r => r.BucketName).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(r => r.FileName).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}