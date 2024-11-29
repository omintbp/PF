using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Queries.GetDiscussionByRelationId;

public class GetDiscussionByRelationIdQueryValidator : AbstractValidator<GetDiscussionByRelationIdQuery>
{
    public GetDiscussionByRelationIdQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid("Page"));

        RuleFor(q => q.PageSize)
            .GreaterThan(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}