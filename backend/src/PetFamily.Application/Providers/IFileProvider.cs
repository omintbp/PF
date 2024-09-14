using CSharpFunctionalExtensions;
using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(Stream stream, string bucketName, string fileName, CancellationToken ct);

    Task<Result<string, Error>> DeleteFile(string bucketName, string fileName, CancellationToken ct);

    Task<Result<string, Error>> GetFile(string bucketName, string fileName, CancellationToken ct);
    
    Task<Result<List<FilePath>, Error>> UploadFiles(IEnumerable<FileData> files, CancellationToken ct);
}