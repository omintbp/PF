using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationQueryValidator : AbstractValidator<GetSpeciesWithPaginationQuery>
{
    public GetSpeciesWithPaginationQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithError(Errors.General.ValueIsInvalid());
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid());
    }
}