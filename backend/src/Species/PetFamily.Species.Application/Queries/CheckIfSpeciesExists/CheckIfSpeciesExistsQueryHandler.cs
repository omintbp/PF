using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.CheckIfSpeciesExists;

public class CheckIfSpeciesExistsQueryHandler : IQueryHandler<bool, CheckIfSpeciesExistsQuery>
{
    private readonly IReadDbContext _readDbContext;

    public CheckIfSpeciesExistsQueryHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<bool, ErrorList>> Handle(
        CheckIfSpeciesExistsQuery query,
        CancellationToken cancellationToken)
    {
        return _readDbContext.Species.Any(b => b.SpeciesId == query.SpeciesId);
    }
}