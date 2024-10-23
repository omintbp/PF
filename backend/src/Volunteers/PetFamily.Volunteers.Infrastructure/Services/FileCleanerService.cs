using Microsoft.Extensions.Logging;
using PetFamily.Core.Messaging;
using PetFamily.Volunteers.Application.Files;
using PetFamily.Volunteers.Application.Providers;
using FileInfo = PetFamily.Volunteers.Application.Providers.FileInfo;

namespace PetFamily.Volunteers.Infrastructure.Services;

public class FileCleanerService : IFileCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _queue;
    private readonly ILogger<FileCleanerService> _logger;

    public FileCleanerService(
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileInfo>> queue,
        ILogger<FileCleanerService> logger)
    {
        _fileProvider = fileProvider;
        _queue = queue;
        _logger = logger;
    }
    
    public async Task Process(CancellationToken cancellationToken)
    {
        var fileInfos = await _queue.ReadAsync(cancellationToken);

        foreach (var info in fileInfos)
        {
            await _fileProvider.DeleteFile(info.BucketName, info.FilePath.Path, cancellationToken);
            _logger.LogInformation("File {FilePath} cleaned from {BucketName}", info.FilePath, info.BucketName);
        }
    }
}