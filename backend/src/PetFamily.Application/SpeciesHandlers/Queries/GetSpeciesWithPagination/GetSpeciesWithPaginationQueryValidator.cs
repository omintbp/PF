using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesHandlers.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationQueryValidator : AbstractValidator<GetSpeciesWithPaginationQuery>
{
    public GetSpeciesWithPaginationQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithError(Errors.General.ValueIsInvalid());
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid());
    }
}