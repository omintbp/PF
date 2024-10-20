using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(Stream stream, string bucketName, string fileName, CancellationToken ct);

    Task<Result<string, Error>> DeleteFile(string bucketName, string fileName, CancellationToken ct);

    Task<Result<string, Error>> GetFile(string bucketName, string fileName, CancellationToken ct);
    
    Task<Result<List<FilePath>, Error>> UploadFiles(IEnumerable<FileData> files, CancellationToken ct);
    
    Task<UnitResult<ErrorList>> DeleteFiles(IEnumerable<FileInfo> files, CancellationToken ct);
}