using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.CheckIfBreedExists;

public class CheckIfBreedExistsQueryHandler : IQueryHandler<bool, CheckIfBreedExistsQuery>
{
    private readonly IReadDbContext _readDbContext;

    public CheckIfBreedExistsQueryHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<bool, ErrorList>> Handle(
        CheckIfBreedExistsQuery query,
        CancellationToken cancellationToken)
    {
        return _readDbContext.Breeds.Any(b =>
            b.BreedId == query.BreedId && b.SpeciesId == query.SpeciesId);
    }
}