using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Application.Queries.GetUnclaimedVolunteerRequestsWithPagination;

public class GetUnclaimedVolunteerRequestsWithPaginationQueryValidator
    : AbstractValidator<GetUnclaimedVolunteerRequestsWithPaginationQuery>
{
    public GetUnclaimedVolunteerRequestsWithPaginationQueryValidator()
    {
        RuleFor(q => q.SortDirection)
            .MaximumLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
            .WithError(Errors.General.ValueIsInvalid("SortDirection"));

        RuleFor(q => q.SortBy)
            .MaximumLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
            .WithError(Errors.General.ValueIsInvalid("SortBy"));

        RuleFor(q => q.Page)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid("Page"));

        RuleFor(q => q.PageSize)
            .GreaterThan(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}