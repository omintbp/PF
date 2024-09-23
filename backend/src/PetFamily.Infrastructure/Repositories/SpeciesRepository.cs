using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.SpeciesHandlers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDbContext _context;

    public SpeciesRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<SpeciesId> Add(Species species, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(species, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return species.Id;
    }

    public async Task<Result<Species, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken = default)
    {
        var species = await _context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);
        
        if (species is null)
            return Errors.General.NotFound(speciesId.Value);

        return species;
    }

    public async Task<Guid> Delete(Species species, CancellationToken cancellationToken = default)
    {
        _context.Remove(species);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return species.Id.Value;
    }
}