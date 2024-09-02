using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Upload;

public class UploadFileRequestHandler
{
    private readonly IFileProvider _fileProvider;

    public UploadFileRequestHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        UploadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var uploadResult = await _fileProvider.UploadFile(
            request.Stream,
            request.BucketName,
            request.FileName,
            cancellationToken);

        return uploadResult;
    }
}