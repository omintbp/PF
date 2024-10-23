using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.CheckIfPetExistsBySpeciesId;

public class CheckIfPetExistsBySpeciesIdQueryHandler 
    : IQueryHandler<bool, CheckIfPetExistsBySpeciesIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public CheckIfPetExistsBySpeciesIdQueryHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<bool, ErrorList>> Handle(
        CheckIfPetExistsBySpeciesIdQuery query, 
        CancellationToken cancellationToken)
    {
        return _readDbContext.Pets.Any(p => p.Species.SpeciesId == query.SpeciesId);
    }
}