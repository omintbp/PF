using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Files;
using PetFamily.Infrastructure.Services;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FileCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FileCleanerBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FileCleanerBackgroundService(
        ILogger<FileCleanerBackgroundService> logger, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FileCleanerBackgroundService is starting.");

        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var fileCleanerService = scope.ServiceProvider.GetRequiredService<IFileCleanerService>();
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            await fileCleanerService.Process(stoppingToken);
        }
    }
}