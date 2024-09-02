using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.Get;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Providers;

namespace PetFamily.API.Controllers;

public class FilesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> UploadFile(
        IFormFile file,
        [FromServices] UploadFileRequestHandler requestHandler,
        [FromServices] UploadFileRequestValidator validator,
        CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();
        
        var request = new UploadFileRequest(stream, "test", file.FileName);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();
        
        var uploadResult = await requestHandler.Handle(request, cancellationToken);

        if (uploadResult.IsFailure)
            return uploadResult.Error.ToResponse();

        return Ok(uploadResult.Value);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFile(
        [FromBody] DeleteFileRequest request,
        [FromServices] DeleteFileRequestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult> GetFile(
        [FromQuery] GetFileRequest request,
        [FromServices] GetFileRequestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}