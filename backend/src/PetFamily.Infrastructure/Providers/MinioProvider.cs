using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
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
}