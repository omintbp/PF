using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;
using FileInfo = PetFamily.Application.Providers.FileInfo;

namespace PetFamily.Application.VolunteersHandlers.Commands.AddPetPhotos;

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