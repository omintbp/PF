using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Get;

public class GetFileRequestHandler
{
    private readonly IFileProvider _fileProvider;

    public GetFileRequestHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        GetFileRequest request,
        CancellationToken cancellationToken)
    {
        var fileResult = await _fileProvider.GetFile(
            request.BucketName,
            request.FileName,
            cancellationToken);

        if (fileResult.IsFailure)
            return fileResult.Error;

        return fileResult.Value;
    }
}