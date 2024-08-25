using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Entities.Volunteers;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly ApplicationDbContext _context;
    
    public VolunteerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(volunteer, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id;
    }
}