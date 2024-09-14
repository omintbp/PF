using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;
using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 4;

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(
        Stream stream,
        string bucketName,
        string fileName,
        CancellationToken cancellationToken)
    {
        var path = Guid.NewGuid();

        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithObject(path.ToString());

            var putObjectResponse = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            _logger.LogInformation("File {fileName} uploaded", fileName);

            return putObjectResponse.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }

    public async Task<Result<string, Error>> DeleteFile(string bucketName, string fileName, CancellationToken ct)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, ct);

            if (bucketExist == false)
                return Error.Failure("file.delete", "Bucket does not exist");

            var statObjectArgs = new StatObjectArgs().WithBucket(bucketName).WithObject(fileName);

            var objectStat = await _minioClient.StatObjectAsync(statObjectArgs, ct);

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, ct);

            _logger.LogInformation("File {fileName} in bucket {bucketName} deleted", fileName, bucketName);

            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete file in minio");
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }
    }

    public async Task<Result<string, Error>> GetFile(string bucketName, string fileName, CancellationToken ct)
    {
        try
        {
            var getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithExpiry(24 * 60 * 60)
                .WithObject(fileName);

            var url = await _minioClient.PresignedGetObjectAsync(getObjectArgs);

            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogInformation("File {fileName} not found in bucket {bucketName}", fileName, bucketName);
                return Errors.General.NotFound();
            }

            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to get file in minio");
            return Error.Conflict("files.get", "Fail to get files in minio");
        }
    }

    public async Task<Result<List<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> files,
        CancellationToken cancellationToken)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = files.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(filesList, cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var results = pathsResult.Select(p => p.Value).ToList();

            _logger.LogInformation("Uploaded files: {files}", results.Select(f => f.Path));

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

            return Error.Failure("file.upload", "Fail to upload files in minio");
        }
    }

    private async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.BucketName)
            .WithStreamData(fileData.Content)
            .WithObjectSize(fileData.Content.Length)
            .WithObject(fileData.FilePath.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FilePath.Path,
                fileData.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient
                .BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}