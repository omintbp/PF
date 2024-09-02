using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Delete;

public class DeleteFileRequestHandler
{
    private readonly IFileProvider _fileProvider;

    public DeleteFileRequestHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        DeleteFileRequest request,
        CancellationToken cancellationToken)
    {
        var deleteFileResult = await _fileProvider.DeleteFile(
            request.BucketName,
            request.ObjectName,
            cancellationToken);

        if (deleteFileResult.IsFailure)
            return deleteFileResult.Error;

        return deleteFileResult.Value;
    }
}