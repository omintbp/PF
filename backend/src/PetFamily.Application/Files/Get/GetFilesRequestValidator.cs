using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Get;

public class GetFilesRequestValidator : AbstractValidator<GetFileRequest>
{
    public GetFilesRequestValidator()
    {
        RuleFor(r => r.FileName).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(r => r.BucketName).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}