using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.Options;

namespace PetFamily.Volunteers.Infrastructure.Services;

public class DeletedPetsCleanerService : IDeletedPetsCleanerService
{
    private readonly ILogger<IDeletedPetsCleanerService> _logger;
    private readonly EntitiesCleanerOptions _options;
    private readonly WriteDbContext _context;

    public DeletedPetsCleanerService(
        ILogger<IDeletedPetsCleanerService> logger,
        IOptions<EntitiesCleanerOptions> options,
        WriteDbContext context)
    {
        _logger = logger;
        _options = options.Value;
        _context = context;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started deleted pets cleaner");

        var volunteers = _context.Volunteers
            .Include(p => p.Pets);

        foreach (var volunteer in volunteers)
        {
            volunteer.DeletePets(p =>
                p.IsDeleted && DateTime.UtcNow > p.DeletionDate.AddDays(_options.ExpiredDaysTime));
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted pets cleaner is done.");
    }
}