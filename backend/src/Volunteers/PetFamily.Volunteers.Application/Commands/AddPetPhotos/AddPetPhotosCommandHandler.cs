using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Application.Providers;
using PetFamily.Volunteers.Domain.Entities;
using FileInfo = PetFamily.Volunteers.Application.Providers.FileInfo;

namespace PetFamily.Volunteers.Application.Commands.AddPetPhotos;

public class AddPetPhotosCommandHandler : ICommandHandler<Guid, AddPetPhotosCommand>
{
    private const string PHOTOS_BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetPhotosCommand> _logger;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _queue;

    public AddPetPhotosCommandHandler(
        IFileProvider fileProvider,
        IVolunteerRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<AddPetPhotosCommand> logger,
        IValidator<AddPetPhotosCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> queue)
    {
        _fileProvider = fileProvider;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
        _queue = queue;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetPhotosCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(volunteerId.Value).ToErrorList();

        var volunteer = volunteerResult.Value;

        var petId = PetId.Create(command.PetId);
        var petResult = volunteer.GetPetById(petId);

        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var pet = petResult.Value;

        List<FileData> filesData = [];

        foreach (var photo in command.Photos)
        {
            var filePathResult = FilePath.Create(
                Guid.NewGuid(),
                Path.GetExtension(photo.FileName));

            if (filePathResult.IsFailure)
                return filePathResult.Error.ToErrorList();

            var filePath = filePathResult.Value;

            var fileData = new FileData(photo.Content, new FileInfo(filePath, PHOTOS_BUCKET_NAME));

            filesData.Add(fileData);
        }

        var filesPathResult = await _fileProvider.UploadFiles(filesData, cancellationToken);

        if (filesPathResult.IsFailure)
        {
            await _queue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);
            
            return filesPathResult.Error.ToErrorList();
        }

        foreach (var path in filesPathResult.Value)
        {
            var petPhotoId = PetPhotoId.NewPetPhotoId();
            var petPhoto = PetPhoto.Create(petPhotoId, path, false);

            if (petPhoto.IsFailure)
                return petPhoto.Error.ToErrorList();

            pet.AddPhoto(petPhoto.Value);
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Uploaded photos to {petId}", petId);

        return volunteerId.Value;
    }
}