using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _context;

    public SpeciesRepository(WriteDbContext context)
    {
        _context = context;
    }
    
    public async Task<SpeciesId> Add(
        Species.Domain.AggregateRoot.Species species, 
        CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(species, cancellationToken);
        
        return species.Id;
    }

    public async Task<Result<Species.Domain.AggregateRoot.Species, Error>> GetById(
        SpeciesId speciesId, 
        CancellationToken cancellationToken = default)
    {
        var species = await _context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);
        
        if (species is null)
            return Errors.General.NotFound(speciesId.Value);

        return species;
    }

    public Guid Delete(Species.Domain.AggregateRoot.Species species)
    {
        _context.Remove(species);
        
        return species.Id.Value;
    }
}