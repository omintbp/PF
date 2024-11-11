using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.Options;

namespace PetFamily.Volunteers.Infrastructure.Services;

public class DeletedVolunteersCleanerService : IDeletedVolunteersCleanerService
{
    private readonly ILogger<IDeletedVolunteersCleanerService> _logger;
    private readonly EntitiesCleanerOptions _options;
    private readonly WriteDbContext _context;

    public DeletedVolunteersCleanerService(
        ILogger<IDeletedVolunteersCleanerService> logger,
        IOptions<EntitiesCleanerOptions> options,
        WriteDbContext context)
    {
        _logger = logger;
        _options = options.Value;
        _context = context;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started deleted volunteers cleaner");

        var volunteers = _context.Volunteers
            .Include(v => v.Pets)
            .ThenInclude(p => p.Photos)
            .Where(v => v.IsDeleted)
            .Where(v => DateTime.UtcNow > v.DeletionDate.AddDays(_options.ExpiredDaysTime));

        _context.Volunteers.RemoveRange(volunteers);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted volunteers cleaner is done.");
    }
}