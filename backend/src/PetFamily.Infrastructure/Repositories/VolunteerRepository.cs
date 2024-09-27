using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersHandlers;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly WriteDbContext _context;

    public VolunteerRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(volunteer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<IEnumerable<Volunteer>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _context.Volunteers
            .Include(v => v.Pets)
            .ThenInclude(p => p.Photos)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers
            .Include(v => v.Pets)
            .ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId.Value);

        return volunteer;
    }

    public async Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _context.Attach(volunteer);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }

    public async Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _context.Remove(volunteer);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }
}