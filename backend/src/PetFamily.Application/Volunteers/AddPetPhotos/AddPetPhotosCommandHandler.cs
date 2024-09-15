using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Providers;
using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public class AddPetPhotosCommandHandler
{
    private const string PHOTOS_BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetPhotosCommand> _logger;
    private readonly IValidator<AddPetPhotosCommand> _validator;

    public AddPetPhotosCommandHandler(
        IFileProvider fileProvider,
        IVolunteerRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<AddPetPhotosCommand> logger,
        IValidator<AddPetPhotosCommand> validator)
    {
        _fileProvider = fileProvider;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddPetPhotosCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return Errors.General.ValueIsInvalid();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(volunteerId.Value);

        var volunteer = volunteerResult.Value;

        var petId = PetId.Create(command.PetId);
        var petResult = volunteer.GetPetById(petId);

        if (petResult.IsFailure)
            return petResult.Error;

        var pet = petResult.Value;

        List<FileData> filesData = [];

        foreach (var photo in command.Photos)
        {
            var filePathResult = FilePath.Create(
                Guid.NewGuid(),
                Path.GetExtension(photo.FileName));

            if (filePathResult.IsFailure)
                return filePathResult.Error;

            var filePath = filePathResult.Value;

            var fileData = new FileData(photo.Content, filePath, PHOTOS_BUCKET_NAME);

            filesData.Add(fileData);
        }

        var filesPathResult = await _fileProvider.UploadFiles(filesData, cancellationToken);

        if (filesPathResult.IsFailure)
            return filesPathResult.Error;

        foreach (var path in filesPathResult.Value)
        {
            var petPhotoId = PetPhotoId.NewPetPhotoId();
            var petPhoto = PetPhoto.Create(petPhotoId, path.Path, false);

            if (petPhoto.IsFailure)
                return petPhoto.Error;

            pet.AddPhoto(petPhoto.Value);
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Uploaded photos to {petId}", petId);

        return volunteerId.Value;
    }
}