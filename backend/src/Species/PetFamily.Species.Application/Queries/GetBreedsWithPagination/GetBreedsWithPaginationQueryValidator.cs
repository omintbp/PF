using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.GetBreedsWithPagination;

public class GetBreedsWithPaginationQueryValidator : AbstractValidator<GetBreedsWithPaginationQuery>
{
    public GetBreedsWithPaginationQueryValidator()
    {
        RuleFor(b => b.SpeciesId).NotEmpty();
        RuleFor(x => x.Page).GreaterThan(0).WithError(Errors.General.ValueIsInvalid());
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid());
    }
}