using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure.Options;

namespace PetFamily.Volunteers.Infrastructure.BackgroundServices;

public class EntitiesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<EntitiesCleanerBackgroundService> _logger;
    private readonly EntitiesCleanerOptions _options;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EntitiesCleanerBackgroundService(
        ILogger<EntitiesCleanerBackgroundService> logger,
        IOptions<EntitiesCleanerOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _options = options.Value;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FileCleanerBackgroundService is starting.");

        while (stoppingToken.IsCancellationRequested == false)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();

            var volunteerCleaner = scope.ServiceProvider.GetRequiredService<IDeletedVolunteersCleanerService>();
            var petsCleaner = scope.ServiceProvider.GetRequiredService<IDeletedPetsCleanerService>();
            
            await petsCleaner.Process(stoppingToken);
            await volunteerCleaner.Process(stoppingToken);

            await Task.Delay(TimeSpan.FromDays(_options.CleaningIntervalDays), stoppingToken);
        }

        _logger.LogInformation("FileCleanerBackgroundService is stopping.");
    }
}