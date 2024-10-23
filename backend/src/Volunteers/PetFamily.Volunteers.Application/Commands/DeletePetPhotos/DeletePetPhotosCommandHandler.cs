using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Volunteers.Application.Providers;
using FileInfo = PetFamily.Volunteers.Application.Providers.FileInfo;

namespace PetFamily.Volunteers.Application.Commands.DeletePetPhotos;

public class DeletePetPhotosCommandHandler : ICommandHandler<DeletePetPhotosCommand>
{
    private const string PHOTOS_BUCKET_NAME = "photos";

    private readonly ILogger<DeletePetPhotosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeletePetPhotosCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _queue;
    private readonly IVolunteerRepository _repository;
    private readonly IFileProvider _fileProvider;

    public DeletePetPhotosCommandHandler(
        ILogger<DeletePetPhotosCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<DeletePetPhotosCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> queue,
        IVolunteerRepository repository,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _queue = queue;
        _repository = repository;
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeletePetPhotosCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);

        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var pet = petResult.Value;

        List<FileInfo> filesInfo = [];

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        
        foreach (var photoId in command.PhotosIds)
        {
            var petPhotoId = PetPhotoId.Create(photoId);
            var photoResult = pet.GetPhotoById(petPhotoId);

            if (photoResult.IsFailure)
                return photoResult.Error.ToErrorList();

            filesInfo.Add(new FileInfo(photoResult.Value.FilePath, PHOTOS_BUCKET_NAME));

            var petPhotoDeleteResult = pet.DeletePhoto(petPhotoId);

            if (petPhotoDeleteResult.IsFailure)
                return petPhotoDeleteResult.Error.ToErrorList();
        }

        await _unitOfWork.SaveChanges(cancellationToken);
        
        var deleteResult = await _fileProvider.DeleteFiles(filesInfo, cancellationToken);

        if (deleteResult.IsFailure)
        {
            transaction.Rollback();
            return deleteResult.Error;
        }
        
        transaction.Commit();

        _logger.LogInformation("Delete pet {petId} photos successfully", command.PetId);

        return new UnitResult<ErrorList>();
    }
}