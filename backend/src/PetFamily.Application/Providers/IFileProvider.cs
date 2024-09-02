using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(Stream stream, string bucketName, string fileName, CancellationToken ct);

    Task<Result<string, Error>> DeleteFile(string bucketName, string fileName, CancellationToken ct);

    Task<Result<string, Error>> GetFile(string bucketName, string fileName, CancellationToken ct);
}