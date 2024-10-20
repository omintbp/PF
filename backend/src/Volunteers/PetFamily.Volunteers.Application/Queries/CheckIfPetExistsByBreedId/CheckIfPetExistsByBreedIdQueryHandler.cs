using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.CheckIfPetExistsByBreedId;

public class CheckIfPetExistsByBreedIdQueryHandler 
    : IQueryHandler<bool, CheckIfPetExistsByBreedIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public CheckIfPetExistsByBreedIdQueryHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<bool, ErrorList>> Handle(
        CheckIfPetExistsByBreedIdQuery query, 
        CancellationToken cancellationToken)
    {
        return _readDbContext.Pets.Any(p => p.Breed.BreedId == query.BreedId);
    }
}