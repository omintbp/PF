using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Queries.CheckIfUserHasPermission;

public class CheckIfUserHasPermissionQueryValidator : AbstractValidator<CheckIfUserHasPermissionQuery>
{
    public CheckIfUserHasPermissionQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(q => q.Permission).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}